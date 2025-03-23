INSERT INTO Categories (Name) VALUES
('Electronics'),
('Fashion'),
('Home & Kitchen'),
('Sports & Outdoors'),
('Books');

INSERT INTO Products (Name, CategoryId) VALUES
-- Electronics
('Smartphone', 1),
('Laptop', 1),
('Wireless Headphones', 1),
('Smartwatch', 1),
('Gaming Console', 1),

-- Fashion
('Mens Jacket', 2),
('Womens Handbag', 2),
('Sneakers', 2),
('Sunglasses', 2),
('Wristwatch', 2),

-- Home & Kitchen
('Non-stick Cookware Set', 3),
('Vacuum Cleaner', 3),
('Microwave Oven', 3),
('Blender', 3),
('Electric Kettle', 3),

-- Sports & Outdoors
('Yoga Mat', 4),
('Dumbbells Set', 4),
('Running Shoes', 4),
('Cycling Helmet', 4),
('Camping Tent', 4),

-- Books
('Fiction Novel', 5),
('Self-Help Guide', 5),
('Science Textbook', 5),
('History Encyclopedia', 5),
('Cooking Recipe Book', 5);
