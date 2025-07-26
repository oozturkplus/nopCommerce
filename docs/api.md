# nopCommerce Web API

The new **nopCommerce Web API** project exposes REST endpoints for managing store data programmatically. It is intended for integrations with mobile apps or third‑party services and uses JWT authentication.

## ShoppingCartApiController

The `ShoppingCartApiController` provides endpoints for working with the shopping cart.

### `GET /api/cart`
Returns the current customer's cart.

```
GET /api/cart HTTP/1.1
Authorization: Bearer {token}
```

Response example:
```json
{
  "items": [
    {"id": 1, "productId": 72, "quantity": 2}
  ],
  "totalQuantity": 2
}
```

### `POST /api/cart/items`
Adds a new item to the cart.

```
POST /api/cart/items HTTP/1.1
Authorization: Bearer {token}
Content-Type: application/json

{
  "productId": 72,
  "quantity": 1
}
```

Response:
```json
{
  "id": 5,
  "productId": 72,
  "quantity": 1
}
```

### `PUT /api/cart/items/{id}`
Updates quantity of an existing cart item.

```
PUT /api/cart/items/5 HTTP/1.1
Authorization: Bearer {token}
Content-Type: application/json

{
  "quantity": 3
}
```

Response:
```json
{
  "id": 5,
  "productId": 72,
  "quantity": 3
}
```

### `DELETE /api/cart/items/{id}`
Removes a cart item.

```
DELETE /api/cart/items/5 HTTP/1.1
Authorization: Bearer {token}
```

Response status: `204 No Content`.
