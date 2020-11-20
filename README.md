# DB_Benchmark

# La tabla Registros en MySQL la cree con esta instrucción

CREATE TABLE `registros` (
  `id` int NOT NULL AUTO_INCREMENT,
  `valor` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

# La tabla Registros en PostgreSQL la cree con esta otra

CREATE TABLE registros (
  id              SERIAL PRIMARY KEY,
  valor           VARCHAR(45) NULL
);
