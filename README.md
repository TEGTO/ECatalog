## Web Application

- **Api Url (Azure)**:  
[https://ecatalog-product-api-germanywestcentral-001.azurewebsites.net/api/v1](https://ecatalog-product-api-germanywestcentral-001.azurewebsites.net/api/v1)
- **Api Url (AWS)**:  
[http://app-productapi-eucentral1-001-env.eba-gbrsxfaq.eu-central-1.elasticbeanstalk.com/](http://app-productapi-eucentral1-001-env.eba-gbrsxfaq.eu-central-1.elasticbeanstalk.com/)
> **Note:** May take a few minutes to load on the first request if the server has been idle.

## Installation Guide

### 1. **Clone the Repository**
   Run the following command to clone the repository:
   ```bash
   git clone https://github.com/TEGTO/ECatalog
   ```

### 2. **Navigate to the Source Directory**
   Move to the directory and open the solution file:
   ```bash
   cd src/ECatalog
   ```
   Open the `.sln` file in your preferred IDE (e.g., Visual Studio).

### 3. **Set Up the Environment**
   - Download and drop to the folder the [.env file](https://drive.google.com/file/d/144dqbbahe3gf86wDv1jTKtK7jKLZ0ZbP/view?usp=drive_link).  
   - Or create one manually with the required environment variables.

### 4. **Run the Project**
   Run the Docker Compose project directly from Visual Studio.

> **Note:** PostgreSQL may take some time to load, and the project automatic migrations may not work on the first run. This happens because the database isn't ready yet. Try running the project again after the database is fully set up.
     
### 5. **Try to use swagger**
   Navigate to the page [http://localhost:7151/swagger/index.html](http://localhost:7151/swagger/index.html)

### 6. **Try to use postman**
Download the [collection](https://github.com/TEGTO/ECatalog/blob/main/postman/ECatalog%20Interaction.postman_collection.json) and open in Postman, collection contains cloud and local endpoint examples.

## Endpoints (before all endpoints is '/api/v1')

### 1. **Create a New Product**
   **Method**: `POST`  
   **Endpoint**: `/products`  
   **Request Body**:
   ```json
   {
     "name": "string",
     "description": "string",
     "price": 0
   }
   ```

### 2. **Get All Products with sorting, pagination and filters**
   **Method**: `GET`  
   **Endpoint**: `/products?search=Product&sortBy=name&descending=true&pageNumber=1&pageSize=10`  

### 3. **Get the Product by the Code**
   **Method**: `GET`  
   **Endpoint**: `/products/{{code}}`  

### 4. **Update the Product**
   **Method**: `PUT`  
   **Endpoint**: `/products`  
   **Request Body**:
   ```json
   {
     "code": "string",
     "name": "string",
     "description": "string",
     "price": 0
   }
   ```

### 5. **Delete the Product**
   **Method**: `DELETE`  
   **Endpoint**: `/products/{{code}}`
   
---
