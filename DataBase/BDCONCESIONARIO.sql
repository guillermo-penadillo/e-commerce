IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'BDCONCESIONARIO')
BEGIN
    CREATE DATABASE BDCONCESIONARIO;
END
GO

USE BDCONCESIONARIO;
GO

-- Crear tabla tbl_pais
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'tbl_pais' AND xtype = 'U')
BEGIN
    CREATE TABLE tbl_pais (
        id_pais INT PRIMARY KEY IDENTITY(1,1),
        nombre_pais VARCHAR(50) NOT NULL UNIQUE
    );
END
GO

-- Crear tabla tbl_marca
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'tbl_marca' AND xtype = 'U')
BEGIN
    CREATE TABLE tbl_marca (
        id_marca INT PRIMARY KEY IDENTITY(1,1),
        nombre_marca VARCHAR(50) NOT NULL,
        id_pais INT NULL,
        FOREIGN KEY (id_pais) REFERENCES tbl_pais(id_pais)
    );
END
GO

-- Crear tabla tbl_combustible
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'tbl_combustible' AND xtype = 'U')
BEGIN
    CREATE TABLE tbl_combustible (
        id_combustible INT PRIMARY KEY IDENTITY(1,1),
        tipo_combustible VARCHAR(30) NOT NULL UNIQUE
    );
END
GO

-- Crear tabla tbl_categoria
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'tbl_categoria' AND xtype = 'U')
BEGIN
    CREATE TABLE tbl_categoria (
        id_categoria INT PRIMARY KEY IDENTITY(1,1),
        nombre_categoria VARCHAR(30) NOT NULL UNIQUE
    );
END
GO

-- Crear tabla tbl_modelo
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'tbl_modelo' AND xtype = 'U')
BEGIN
    CREATE TABLE tbl_modelo (
        id_modelo INT PRIMARY KEY IDENTITY(1,1),
        id_marca INT NOT NULL,
        nombre_modelo VARCHAR(50) NOT NULL,
        anio INT NOT NULL,
        id_combustible INT NOT NULL,
		id_categoria INT NOT NULL,
		FOREIGN KEY (id_marca) REFERENCES tbl_marca(id_marca),
		FOREIGN KEY (id_combustible) REFERENCES tbl_combustible(id_combustible),
		FOREIGN KEY (id_categoria) REFERENCES tbl_categoria(id_categoria)
    );
END
GO

-- Crear tabla tbl_color
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'tbl_color' AND xtype = 'U')
BEGIN
	CREATE TABLE tbl_color (
		id_color INT PRIMARY KEY IDENTITY(1,1),
		nombre_color VARCHAR(30) NOT NULL UNIQUE
	);
END
GO

-- Crear tabla tbl_traccion
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'tbl_transmision' AND xtype = 'U')
BEGIN
	CREATE TABLE tbl_transmision (
		id_transmision  INT PRIMARY KEY IDENTITY(1,1),
		nombre_transmision  VARCHAR(30) NOT NULL UNIQUE
	);
END
GO

-- Crear tabla tbl_vehiculo
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'tbl_vehiculo' AND xtype = 'U')
BEGIN
    CREATE TABLE tbl_vehiculo (
        id_vehiculo INT PRIMARY KEY IDENTITY(1,1),
        id_modelo INT NOT NULL,
        id_color INT NOT NULL,
		id_transmision INT NOT NULL,
        precio DECIMAL(10,2) NOT NULL,
        stock INT NOT NULL,
        FOREIGN KEY (id_modelo) REFERENCES tbl_modelo(id_modelo),
		FOREIGN KEY (id_color) REFERENCES tbl_color(id_color),
		FOREIGN KEY (id_transmision) REFERENCES tbl_transmision(id_transmision)
    );
END
GO


-- Crear tabla tbl_usuario 
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'tbl_usuario' AND xtype = 'U')
BEGIN
    CREATE TABLE tbl_usuario (
        id_usuario INT PRIMARY KEY IDENTITY(1,1),
        nombre_usuario VARCHAR(50) NOT NULL UNIQUE,
        password VARCHAR(50) NOT NULL,
    );
END
GO

-- Inserts en tbl_pais
INSERT INTO tbl_pais (nombre_pais) 
VALUES 
('Japón'),
('Estados Unidos'),
('Alemania'),
('Francia');
GO

-- Inserts en tbl_combustible
INSERT INTO tbl_combustible (tipo_combustible) 
VALUES 
('Gasolina'),
('Diesel'),
('Híbrido'),
('Eléctrico');
GO

-- Inserts en tbl_categoria
INSERT INTO tbl_categoria (nombre_categoria)
VALUES 
('Sedán'),
('SUV'),
('Deportivo'),
('Pickup');
GO

-- Inserts en tbl_marca
INSERT INTO tbl_marca (nombre_marca, id_pais) 
VALUES 
('Toyota', 1), -- Japón
('Ford', 2), -- Estados Unidos
('BMW', 3); -- Alemania
GO

-- Inserts en tbl_modelo
INSERT INTO tbl_modelo (id_marca, nombre_modelo, anio, id_combustible, id_categoria)
VALUES 
(1, 'Corolla', 2022, 1, 1), -- Toyota Corolla, Gasolina, Sedán
(1, 'Hilux', 2021, 2, 4), -- Toyota Hilux, Diesel, Pickup
(2, 'Mustang', 2023, 1, 3), -- Ford Mustang, Gasolina, Deportivo
(3, 'Serie 3', 2022, 3, 1); -- BMW Serie 3, Híbrido, Sedán
GO

-- Inserts en tbl_color
INSERT INTO tbl_color(nombre_color) 
VALUES 
('Rojo'),
('Blanco'),
('Negro'),
('Azul');
GO

-- Inserts en tbl_transmision
INSERT INTO tbl_transmision (nombre_transmision)
VALUES
('Manual'),
('Automático');
GO


-- Inserts en tbl_vehiculo
INSERT INTO tbl_vehiculo (id_modelo, id_color, id_transmision, precio, stock) 
VALUES 
(1, 1, 1, 22000.00, 10), -- Corolla, Rojo, Manual, 22000, 10 en stock
(2, 2, 2, 30000.00, 5), -- Hilux, Blanco, Automático, 30000, 5 en stock
(3, 3, 1, 40000.00, 2), -- Mustang, Negro, Manual, 40000, 2 en stock
(4, 4, 2, 45000.00, 3); -- Serie 3, Azul, Automático, 45000, 3 en stock
GO

-- Inserts en tbl_usuario
INSERT INTO tbl_usuario (nombre_usuario, password) 
VALUES 
('admin', 'admin123'),
('johndoe', 'password123'),
('janedoe', 'securepass');
GO

--
-- Procesos almacenados
--
CREATE OR ALTER PROCEDURE sp_listar_vehiculo
AS
BEGIN
    SELECT 
        v.id_vehiculo,
        mc.nombre_marca,
        m.nombre_modelo,
        c.tipo_combustible,
        cl.nombre_color,
        t.nombre_transmision, 
        cat.nombre_categoria, 
        v.precio,
        v.stock
    FROM tbl_vehiculo v
    INNER JOIN tbl_modelo m ON v.id_modelo = m.id_modelo
    INNER JOIN tbl_combustible c ON m.id_combustible = c.id_combustible
    INNER JOIN tbl_color cl ON v.id_color = cl.id_color
    INNER JOIN tbl_marca mc ON m.id_marca = mc.id_marca
    LEFT JOIN tbl_categoria cat ON m.id_categoria = cat.id_categoria
    LEFT JOIN tbl_transmision t ON v.id_transmision = t.id_transmision
END;
GO
--
--
CREATE OR ALTER PROCEDURE sp_merge_vehiculo
    @id_vehiculo INT,
    @id_modelo INT,
    @id_color INT,
    @id_transmision INT,
    @precio DECIMAL(10,2),
    @stock INT
AS
BEGIN
    IF EXISTS (SELECT * FROM tbl_vehiculo WHERE id_vehiculo = @id_vehiculo)
    BEGIN
        -- Actualizar disco existente por ID
        UPDATE tbl_vehiculo
        SET id_modelo = @id_modelo,
            id_color= @id_color,
            id_transmision = @id_transmision,
            precio = @precio,
            stock = @stock
        WHERE id_vehiculo = @id_vehiculo;
    END
    ELSE
    BEGIN
        -- Insertar nuevo disco si no existe
        INSERT INTO tbl_vehiculo(id_modelo, id_color, id_transmision, precio, stock)
        VALUES (@id_modelo, @id_color, @id_transmision, @precio, @stock);
    END
END;
GO
--
--
CREATE OR ALTER PROCEDURE sp_eliminar_vehiculo
    @id_vehiculo INT
AS
BEGIN
    DELETE FROM tbl_vehiculo
    WHERE id_vehiculo = @id_vehiculo;
END;
GO
--
--
CREATE OR ALTER PROCEDURE sp_seguridad_usuario
    @usuario VARCHAR(255),
    @password VARCHAR(255)
AS
BEGIN
	SELECT nombre_usuario,password FROM tbl_usuario WHERE nombre_usuario=@usuario and password=@password;
END;
GO
--
--
