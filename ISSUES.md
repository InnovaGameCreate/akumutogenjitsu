## Discussion on Save System Design

### ISaveableManager Interface Approach
The ISaveableManager interface is designed to abstract the save/load processes for different manager types in our system. This allows for flexibility and scalability as we add new manager types in the future. Each manager that implements this interface will define its own logic for saving and loading data, ensuring a consistent approach across the application.

### Unified SaveManager
The unified SaveManager acts as the central point for managing all save operations. It is responsible for:
- **JSON to Base64 Encoding/Decoding**: This functionality allows us to convert our JSON data into a Base64 string for storage, which is more compact and easier to handle in certain scenarios. 
- **Handling Different Manager Types**: By utilizing the ISaveableManager interface, the SaveManager can interact with various manager types seamlessly, calling their specific save/load methods as needed.

### Conclusion
This design not only meets the requirements outlined in issue #117 but also lays the groundwork for a robust and adaptable save system that can evolve with our project's needs.