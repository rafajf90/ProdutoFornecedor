# ProdutoFornecedor
CRUD - Teste Prático - Certsys


CREATE TABLE Fornecedor(
  IdFornecedor Integer primary key IDENTITY (1,1) not null,  
  Nome_Fornecedor Varchar(60) not null,
	CNPJ Varchar(14) not null,
	Endereco Varchar(100) not null,
	Ativo Bit not null
	);
	
CREATE TABLE Produto(
	IdProduto Integer primary key IDENTITY (1,1) not null,
	Nome_Fornecedor Varchar(60) not null,
	NomeProduto Varchar(50) not null,
	Quantidade Integer not null,	
	);
	
CREATE TABLE Usuario(
	IdUsuario Integer primary key IDENTITY (1,1) not null,
	NomeUsuario Varchar(50) not null,
	SenhaUsuario Varchar(50) not null,
	);
