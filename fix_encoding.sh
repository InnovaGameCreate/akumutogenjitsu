#!/usr/bin/env bash
# fix_encoding.sh
# リポジトリ内のテキストファイルを走査し、文字化け（U+FFFD '�'）を含むファイルを
# バックアップした上で、一般的な日本語エンコーディングから UTF-8 に自動変換します。
#
# 使い方:
#   .fix_encoding.sh            # 現在のディレクトリ (repo root) を対象に実行
#   .fix_encoding.sh -n         # ドライラン: 対象ファイル一覧を表示するのみ
#   .fix_encoding.sh path/to/dir # 指定ディレクトリを対象
#
# 注意:
# - 自動変換は iconv を利用します。macOS に標準で入っています。
# - 変換前にバックアップを作成します (./encoding_backups/<timestamp>/...)
# - すべてのケースで完璧に復元できる保証はありません。重要なファイルは必ず差分を確認してください。

set -euo pipefail

ROOT_DIR=${1:-.}
DRY_RUN=0

# 認識する拡張子 (必要なら追加してください)
EXTS=(cs meta txt md shader asmdef json xml js ts csproj cfg ini yml yaml)

# 変換候補エンコーディングの順序 (良く使うものから)
ENCODINGS=(CP932 SHIFT_JIS SHIFT_JIS_2004 MS_KANJI EUC-JP ISO-2022-JP UTF-16 UTF-16LE UTF-16BE)

TIMESTAMP=$(date +%Y%m%d_%H%M%S)
BACKUP_DIR="encoding_backups/${TIMESTAMP}"

usage() {
  echo "Usage: $0 [-n] [path]"
  echo "  -n   dry run (no files changed)"
  exit 1
}

if [[ "${ROOT_DIR}" == "-n" ]]; then
  DRY_RUN=1
  ROOT_DIR=${2:-.}
fi

# check commands
command -v iconv >/dev/null 2>&1 || { echo "iconv is required but not found" >&2; exit 1; }
command -v file >/dev/null 2>&1 || { echo "file is required but not found" >&2; exit 1; }

# build find pattern
FIND_EXPR=()
for ext in "${EXTS[@]}"; do
  FIND_EXPR+=( -o -iname "*.$ext" )
done
# remove leading -o
FIND_EXPR=( "(" "${FIND_EXPR[@]:1}" ")" )

# collect files (portable)
FILES=()
while IFS= read -r -d $'\0' file; do
  FILES+=("$file")
done < <(find "$ROOT_DIR" -type f "${FIND_EXPR[@]}" -print0 2>/dev/null || true)

if [[ ${#FILES[@]} -eq 0 ]]; then
  echo "No candidate files found under '$ROOT_DIR'." >&2
  exit 0
fi

echo "Found ${#FILES[@]} candidate files. Scanning for replacement character U+FFFD..."

# Helper to check for U+FFFD
contains_replacement() {
  local f="$1"
  # use grep with literal unicode char
  if grep -q $'\uFFFD' "$f"; then
    return 0
  else
    return 1
  fi
}

# Prepare backup dir if not dry run
if [[ $DRY_RUN -eq 0 ]]; then
  mkdir -p "$BACKUP_DIR"
fi

converted_count=0
failed=()

for f in "${FILES[@]}"; do
  # skip binary files by quick heuristic
  # file -b --mime-encoding
  enc=$(file -b --mime-encoding "$f" 2>/dev/null || echo "unknown")

  # if file reports binary or unknown, skip
  if [[ "$enc" == "binary" || "$enc" == "unknown" ]]; then
    continue
  fi

  if contains_replacement "$f"; then
    echo "\n[FOUND] $f contains U+FFFD (replacement char)"

    if [[ $DRY_RUN -eq 1 ]]; then
      continue
    fi

    # backup
    dest="$BACKUP_DIR/${f#./}"
    mkdir -p "$(dirname "$dest")"
    cp -p "$f" "$dest"

    tmpfile=$(mktemp)
    converted=0

    for enc_try in "${ENCODINGS[@]}"; do
      # try to convert
      if iconv -f "$enc_try" -t UTF-8 "$f" -o "$tmpfile" 2>/dev/null; then
        # ensure no replacement char after conversion
        if ! grep -q $'\uFFFD' "$tmpfile"; then
          mv "$tmpfile" "$f"
          echo "Converted $f from $enc_try -> UTF-8"
          converted=1
          ((converted_count++))
          break
        fi
      fi
    done

    if [[ $converted -eq 0 ]]; then
      # try using file-detected encoding if it's not utf-8
      if [[ "$enc" != "utf-8" && "$enc" != "us-ascii" ]]; then
        if iconv -f "$enc" -t UTF-8 "$f" -o "$tmpfile" 2>/dev/null; then
          if ! grep -q $'\uFFFD' "$tmpfile"; then
            mv "$tmpfile" "$f"
            echo "Converted $f from detected encoding ($enc) -> UTF-8"
            converted=1
            ((converted_count++))
          fi
        fi
      fi
    fi

    if [[ $converted -eq 0 ]]; then
      echo "Failed to auto-convert $f. Backup stored at $dest"
      failed+=("$f")
      rm -f "$tmpfile" || true
    fi
  fi
done

echo "\nSummary:"
echo "  Converted: $converted_count"
if [[ ${#failed[@]} -gt 0 ]]; then
  echo "  Failed: ${#failed[@]}"
  for ff in "${failed[@]}"; do
    echo "    - $ff"
  done
fi

echo "Backups (if any) are under: ${BACKUP_DIR}"

if [[ $DRY_RUN -eq 0 ]]; then
  echo "You should review changes (git diff) and commit when satisfied."
fi
