###  Team Members : Doina Plesca
## 🔗 Public Repository

This project is hosted at: [https://github.com/DoinaPlesca/EcommerceApp](https://github.com/DoinaPlesca/EcommerceApp)


# 🛍️ EcommerceApp – Backend for Second-Hand Items Platform

This is a backend API for an online platform where users can buy and sell second-hand items. It’s built with **.NET Core**, **MongoDB**, **Redis**, and **Cloudinary**. The project uses a clean and scalable architecture with **CQRS**, clear folder separation, validation, and caching.

---

## 🗂️ Project Structure

The project is organized by feature, following CQRS principles.


```plaintext
EcommerceApp/
│
├── Configuration/             # Settings for MongoDB, Cloudinary
│
├── Controllers/               # API endpoints
│   ├── ListingsController.cs
│   ├── OrdersController.cs
│   ├── ReviewsController.cs
│   ├── UsersController.cs
│   └── UploadController.cs
│
├── Features/                  # Feature folders following CQRS
│   ├── Listings/
│   │   ├── Commands/
│   │   ├── Queries/
│   │   ├── Handlers/
│   │   └── Validators/
│   ├── Orders/
│   │   ├── Commands/
│   │   ├── Handlers/
│   │   └── Validators/
│   ├── Reviews/
│   │   ├── Commands/
│   │   ├── Queries/
│   │   ├── Handlers/
│   │   └── Validators/
│   └── Users/
│       ├── Queries/
│       ├── Handlers/
│       └── Validators/
│
├── Middleware/                # Global error handling
│   └── ErrorHandlerMiddleware.cs
│
├── Models/                    # Data models and DTOs
│   ├── DTOs/
│   ├── Enums/
│   ├── Listing.cs
│   ├── Order.cs
│   ├── Review.cs
│   ├── User.cs
│   ├── ApiResponse.cs
│   └── PagedResult.cs
│
├── Services/                  # External integrations
│   ├── CloudinaryService.cs
│   ├── MongoService.cs
│   └── RedisCacheService.cs
│
├── Shared/                    # Shared constants or utilities
│   └── Constants/
│
├── appsettings.json           # App configuration file
├── docker-compose.yml         # MongoDB, Redis containers
└── Program.cs                 # Main application entry point

```
---

## 📦 Technologies Used

| Purpose              | Technology               |
|----------------------|--------------------------|
| Backend Framework    | .NET Core                |
| NoSQL Database       | MongoDB                  |
| Caching              | Redis                    |
| File Uploads         | Cloudinary               |
| CQRS Pattern         | MediatR                  |
| Validation           | FluentValidation         |
| API Documentation    | Swagger                  |
| Container Management | Docker Compose           |

---
## ⚙️ Architecture

- **MongoDB** stores flexible data like listings, orders, users, and reviews.
- **Cloudinary** handles image uploads, keeping the database fast and light.
- **Redis** improves speed for repeated data reads.
- **MediatR** + **CQRS** cleanly separates read (queries) and write (commands) logic.
- **FluentValidation** is used to check inputs for commands and queries.

---

## 🎯 Platform Features

### 📤 Create Listings
- Users can list items for sale by submitting title, price, description, category, condition, and images.
- Images are uploaded to Cloudinary and saved as URLs in the database.
- All listings start with a default status: `"Available"`.
- Listings are managed through a `CreateListingCommand` and validated via FluentValidation. They can later be updated to change their status (e.g. marked as "Sold").

### 🔍 Browse Listings
- Users can search listings by title.
- Filter by category or item status (e.g., available, sold).
- Sort results by price or creation date.
- Supports pagination.
- Paginated results improve performance by limiting how much data is returned at once. This is especially useful as the number of listings grows, and it helps frontend apps load listings efficiently.

### 🛒 Place Orders
- Buyers can order available items.
- The system checks if the listing exists and is still unsold.
- Prevents duplicate orders.
- This logic is handled in the `PlaceOrderCommand` and `PlaceOrderHandler`.

### 🔄 Update Order Status
- The status of an order can be updated (e.g., from `"Pending"` to `"Completed"` or `"Cancelled"`).

### ⭐ Leave Reviews
- Buyers can leave one review per order.
- Reviews include a 1–5 rating and a comment.
- Reviews are tied to sellers and used to calculate seller ratings.

### 👤 View Seller Profiles
- Each seller has a profile that includes their name and average rating.

### 🖼️ Upload Images
- Supports uploading one or more images per listing.
- Images are stored in Cloudinary and linked via URL in MongoDB.

---

## 🔧 Design & Implementation Justification

### 1️⃣ Database Selection

We use **MongoDB** (NoSQL) exclusively for all collections:

| Data Type     | Collection       | Reason for NoSQL |
|---------------|------------------|------------------|
| Listings      | `Listings`       | User-generated, flexible schema |
| Orders        | `Orders`         | High write volume, denormalized |
| Users         | `Users`          | Lightweight profile storage |
| Reviews       | `Reviews`        | Direct reference by Order/Seller |

**Why MongoDB?**

We use MongoDB (NoSQL) for all collections because it's ideal for flexible, user-generated data. Listings may have different attributes or image counts, which Mongo handles well due to its dynamic schema. It also supports fast read/write operations and horizontal scaling, which are important for a growing e-commerce platform.

---

### 2️⃣ Data Schema 

Each entity is modeled as a collection with clear ownership and references:

- **Listing**
    - `Id`, `Title`, `Description`, `Price`, `SellerId`, `ImageUrls`, `Status`, `Condition`,`Category`, `CreateAt`
- **Order**
    - `Id`, `BuyerId`, `SellerId`, `ListingId`, `TotalPrice`, `Status`,`OrderedAt`
- **Review**
    - `OrderId`, `SellerId`, `BuyerId`, `Rating`, `Comment`
- **User**
    - `Id`, `Name`, `Rating`

All listing images are stored externally via Cloudinary. Only the URL is persisted in MongoDB.

---

### 3️⃣ Cloud Storage Integration

We use **Cloudinary** to handle media content:

- Users upload images via the `/api/upload` endpoint.
- Images are uploaded to Cloudinary using its SDK.
- Cloudinary returns a secure URL, which is stored in the `ImageUrls` field of a listing.

**Why Cloudinary?**
Cloudinary simplifies image upload and management. It handles resizing, CDN delivery, and optimization. This reduces backend storage concerns and improves image loading performance on the frontend.

---

### 4️⃣ Caching Strategy

We implemented **Redis** caching using the StackExchange.Redis library.

| Cached Data               | Key Format                       | Expiration      |
|---------------------------|----------------------------------|------------------|
| User Profile              | `user:{userId}`                  | 15 min           |
| Listing by ID             | `listing:{listingId}`            | 15 min           |
| Listings by Seller        | `listings:seller:{sellerId}`     | 15 min           |
| Seller Reviews            | `reviews:seller:{sellerId}`      | 15 min           |

**Cache Invalidation**:

- When a listing is created or updated, its related cache (`listings:seller:*`) is invalidated.
- When a review is added, we invalidate `reviews:seller:*`.

##### Why Redis?

Redis is chosen for its in-memory speed and ease of use. We cache frequently accessed data (like listings and users) to reduce database load and improve response times. We manually control invalidation to avoid stale data issues.

---

### 5️⃣ CQRS Implementation (MediatR)

We use **MediatR** to enforce CQRS architecture:

- **Commands** perform write operations:
    - `CreateListingCommand`
    - `UpdateListingStatus`
    - `PlaceOrderCommand`
    - `UpdateOrderStatus`
    - `CreateReviewCommand`
- **Queries** perform reads:
    - `GetListingsQuery`
    - `GetListingsBySeller`
    - `GetListingsById`
    - `GetUserQuery`
    - `GetReviewsBySellerQuery`

**Why CQRS?**
This separation ensures:
- Better scalability(read/write separation)
- Clean structure for unit testing
- Isolation of side effects

Each handler implements `IRequestHandler<TRequest, TResponse>` and is validated with **FluentValidation**.

---

### 6️⃣ Transaction Management

We ensure transactional safety by combining **strict business rule validation** with **MongoDB’s atomic single-document writes**.

#### Where We Ensure Consistency

| Operation               | Handler                          | Logic Ensured                                                                 |
|------------------------|----------------------------------|-------------------------------------------------------------------------------|
| **Place Order**        | `PlaceOrderHandler.cs`           | Validates listing exists and hasn't been ordered. Prevents duplicate orders. |
| **Update Listing**     | `UpdateListingStatusHandler.cs`  | Ensures listing exists and updates its status atomically.                    |
| **Create Review**      | `CreateReviewHandler.cs`         | Confirms order ownership and enforces one-review-per-order constraint.       |

####  How We Enforce Transactional Safety

-  *Business rules are validated before database writes*
- *Invalid operations are rejected early*
-  *Only one document is updated per operation*
- *No need for manual rollback logic*

This approach keeps the application **fast, safe, and predictable**, while still maintaining full data integrity.

####  Why No Multi-Document Transactions?

MongoDB saves one document at a time completely, so our operations stay safe and consistent without needing complex multi-document transactions.

> **Note:** If in the future we need to update multiple documents (e.g., reserve a listing and create an order simultaneously), we can implement MongoDB’s native multi-document transactions using `StartSession()` and `StartTransaction()`.

---
## 🧾 API Response Format

All API responses follow this format:

### ✅ Success

```json
{
  "success": true,
  "message": "Files uploaded.",
  "data": [
    "https://cloudinary.com/image1.jpg",
    "https://cloudinary.com/image2.jpg"
  ]
}
```
### ❌ Error
```json
{
  "success": false,
  "message": "Item not found",
  "data": null
}
```
## ▶️ Run the Project
Start MongoDB & Redis with Docker
```bash
docker-compose up -d
```

Run .NET API
```bash
dotnet run
```



