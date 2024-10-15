CREATE TABLE Car (
    Id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NOT NULL,
    mpg NVARCHAR(255) NOT NULL,
    cylinders NVARCHAR(255) NOT NULL,
    displacement NVARCHAR(255) NOT NULL,
    horsepower NVARCHAR(255) NOT NULL,
    weight NVARCHAR(255) NOT NULL,
    acceleration NVARCHAR(255) NOT NULL,
    model_year NVARCHAR(255) NOT NULL,
    origin NVARCHAR(255) NOT NULL,
    is_deleted int NULL
);

