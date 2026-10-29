USE personalplans;
INSERT INTO Activities (Name, Description, Category, CostPerPerson, DurationMinutes, SuitableCompanies, IsActive, CreatedAt)
VALUES
('Picnic en el parque', 'Picnic al aire libre en un parque', 'Aire libre', 20.00, 120, 'Pareja,Amigos,Familia', 1, UTC_TIMESTAMP()),
('Caminata', 'Caminata recreativa', 'Deporte', 0.00, 90, 'Solo,Pareja,Amigos,Familia', 1, UTC_TIMESTAMP()),
('Noche de películas en casa', 'Ver una película en casa', 'Entretenimiento', 0.00, 120, 'Solo,Pareja,Amigos,Familia', 1, UTC_TIMESTAMP()),
('Cafetería', 'Visita a una cafetería', 'Comida', 25.00, 60, 'Solo,Pareja,Amigos', 1, UTC_TIMESTAMP()),
('Museo', 'Visita a un museo', 'Cultura', 15.00, 120, 'Solo,Pareja,Amigos,Familia', 1, UTC_TIMESTAMP());
