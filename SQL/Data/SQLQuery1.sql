
DELETE MeProduct WHERE ID > 0
DELETE Product WHERE ID > 0

---DELETE MeProduct WHERE ID = '7972'
SELECT * FROM MeProduct WHERE ID = '7972'
---DELETE Product WHERE ID = '7970'
SELECT * FROM Product WHERE ID = '7971'

SELECT COUNT(*) FROM MeProduct
SELECT COUNT(*) FROM Product
SELECT COUNT(*) FROM Location

SELECT * FROM Location WHERE ID = 4923

UPDATE MeProduct SET IsStock = 1 WHERE ID > 0