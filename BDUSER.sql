use DBCRUDCORE;

create taBLE USUARIO(
	IdUsuario int primary key identity,
	Nombre varchar(50),
	Correo varchar(50),
	Clave varchar(50),
	Restablecer bit,
	Confirmado bit,
	Token varchar(200)
);