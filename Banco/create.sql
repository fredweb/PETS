-- MySQL Script generated by MySQL Workbench
-- Sun Jan 28 12:34:55 2018
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema PETS
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `PETS` ;

-- -----------------------------------------------------
-- Schema PETS
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `PETS` DEFAULT CHARACTER SET utf8 ;
SHOW WARNINGS;
USE `PETS` ;

-- -----------------------------------------------------
-- Table `PETS`.`SEXO`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`SEXO` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`SEXO` (
  `SQSEXO` INT NOT NULL AUTO_INCREMENT COMMENT 'Chave Primaria',
  `DSSEXO` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`SQSEXO`))
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`CLIENTE`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`CLIENTE` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`CLIENTE` (
  `SQCLIENTE` INT NOT NULL AUTO_INCREMENT COMMENT 'chave primaria ',
  `NMNOME` VARCHAR(255) NOT NULL COMMENT 'nome do cliente ',
  `DTNASCIMENTO` DATE NOT NULL COMMENT 'idade',
  `SQSEXO` INT NOT NULL,
  PRIMARY KEY (`SQCLIENTE`),
  INDEX `fk_Cliente_Sexo_idx` (`SQSEXO` ASC),
  CONSTRAINT `fk_Cliente_Sexo`
    FOREIGN KEY (`SQSEXO`)
    REFERENCES `PETS`.`SEXO` (`SQSEXO`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`TIPOFUNCIONARIO`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`TIPOFUNCIONARIO` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`TIPOFUNCIONARIO` (
  `SQTIPOFUNCIONARIO` INT NOT NULL AUTO_INCREMENT,
  `DSTIPO` VARCHAR(100) NOT NULL,
  `SGTIPO` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`SQTIPOFUNCIONARIO`),
  UNIQUE INDEX `SGTIPO_UNIQUE` (`SGTIPO` ASC))
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`FUNCIONARIO`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`FUNCIONARIO` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`FUNCIONARIO` (
  `SQFUNCIONARIO` INT NOT NULL AUTO_INCREMENT,
  `SQTIPOFUNCIONARIO` INT NOT NULL,
  `NMNOME` VARCHAR(255) NOT NULL,
  `DTNASCIMENTO` DATE NOT NULL,
  `DSTELEFONE` VARCHAR(20) NOT NULL,
  `NUCPF` VARCHAR(13) NOT NULL,
  `SQSEXO` INT NOT NULL,
  PRIMARY KEY (`SQFUNCIONARIO`),
  INDEX `fk_Funcionario_Sexo1_idx` (`SQSEXO` ASC),
  INDEX `fk_Funcionario_TIPOFUNCIONARIO1_idx` (`SQTIPOFUNCIONARIO` ASC),
  CONSTRAINT `fk_Funcionario_Sexo1`
    FOREIGN KEY (`SQSEXO`)
    REFERENCES `PETS`.`SEXO` (`SQSEXO`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Funcionario_TIPOFUNCIONARIO1`
    FOREIGN KEY (`SQTIPOFUNCIONARIO`)
    REFERENCES `PETS`.`TIPOFUNCIONARIO` (`SQTIPOFUNCIONARIO`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`TIPODOCUMENTO`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`TIPODOCUMENTO` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`TIPODOCUMENTO` (
  `SQTIPODOCUMENTO` INT NOT NULL AUTO_INCREMENT,
  `NMDOCUMENTO` VARCHAR(255) NOT NULL,
  `SGDOCUMENTO` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`SQTIPODOCUMENTO`),
  UNIQUE INDEX `SGDOCUMENTO_UNIQUE` (`SGDOCUMENTO` ASC))
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`TIPODOCUMENTOFUNCIONARIO`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`TIPODOCUMENTOFUNCIONARIO` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`TIPODOCUMENTOFUNCIONARIO` (
  `SQTIPODOCUMENTO` INT NOT NULL,
  `SQFUNCIONARIO` INT NOT NULL,
  `VLVALOR` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`SQTIPODOCUMENTO`, `SQFUNCIONARIO`),
  INDEX `fk_TIPODOCUMENTO_has_FUNCIONARIO_FUNCIONARIO1_idx` (`SQFUNCIONARIO` ASC),
  INDEX `fk_TIPODOCUMENTO_has_FUNCIONARIO_TIPODOCUMENTO1_idx` (`SQTIPODOCUMENTO` ASC),
  CONSTRAINT `fk_TIPODOCUMENTO_has_FUNCIONARIO_TIPODOCUMENTO1`
    FOREIGN KEY (`SQTIPODOCUMENTO`)
    REFERENCES `PETS`.`TIPODOCUMENTO` (`SQTIPODOCUMENTO`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_TIPODOCUMENTO_has_FUNCIONARIO_FUNCIONARIO1`
    FOREIGN KEY (`SQFUNCIONARIO`)
    REFERENCES `PETS`.`FUNCIONARIO` (`SQFUNCIONARIO`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`FORNECEDOR`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`FORNECEDOR` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`FORNECEDOR` (
  `SQFORNECEDOR` INT NOT NULL AUTO_INCREMENT,
  `NMFANTASIA` VARCHAR(100) NOT NULL,
  `NMRAZAOSOCIAL` VARCHAR(100) NOT NULL,
  `DSTELEFONE` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`SQFORNECEDOR`))
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`TIPODOCUMENTOFORNECEDOR`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`TIPODOCUMENTOFORNECEDOR` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`TIPODOCUMENTOFORNECEDOR` (
  `SQTIPODOCUMENTO` INT NOT NULL,
  `SQFORNECEDOR` INT NOT NULL,
  `VLVALOR` VARCHAR(200) NOT NULL,
  PRIMARY KEY (`SQTIPODOCUMENTO`, `SQFORNECEDOR`),
  INDEX `fk_TIPODOCUMENTO_has_FORNECEDOR_FORNECEDOR1_idx` (`SQFORNECEDOR` ASC),
  INDEX `fk_TIPODOCUMENTO_has_FORNECEDOR_TIPODOCUMENTO1_idx` (`SQTIPODOCUMENTO` ASC),
  CONSTRAINT `fk_TIPODOCUMENTO_has_FORNECEDOR_TIPODOCUMENTO1`
    FOREIGN KEY (`SQTIPODOCUMENTO`)
    REFERENCES `PETS`.`TIPODOCUMENTO` (`SQTIPODOCUMENTO`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_TIPODOCUMENTO_has_FORNECEDOR_FORNECEDOR1`
    FOREIGN KEY (`SQFORNECEDOR`)
    REFERENCES `PETS`.`FORNECEDOR` (`SQFORNECEDOR`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`CATEGORIA`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`CATEGORIA` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`CATEGORIA` (
  `SQCATEGORIA` INT NOT NULL AUTO_INCREMENT,
  `DSCATEGORIA` VARCHAR(255) NOT NULL,
  `SGCATEGORIA` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`SQCATEGORIA`),
  UNIQUE INDEX `SGCATEGORIA_UNIQUE` (`SGCATEGORIA` ASC))
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`MATERIAL`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`MATERIAL` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`MATERIAL` (
  `SQMATERIAL` INT NOT NULL AUTO_INCREMENT,
  `SQCATEGORIA` INT NOT NULL,
  `NMNOME` VARCHAR(100) NOT NULL,
  `DSDESCRICAO` VARCHAR(500) NOT NULL,
  PRIMARY KEY (`SQMATERIAL`),
  INDEX `fk_MATERIAL_CATEGORIA1_idx` (`SQCATEGORIA` ASC),
  CONSTRAINT `fk_MATERIAL_CATEGORIA1`
    FOREIGN KEY (`SQCATEGORIA`)
    REFERENCES `PETS`.`CATEGORIA` (`SQCATEGORIA`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`ESTOQUE`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`ESTOQUE` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`ESTOQUE` (
  `SQESTOQUE` INT NOT NULL AUTO_INCREMENT,
  `SQFORNECEDOR` INT NOT NULL,
  `SQMATERIAL` INT NOT NULL,
  `NUREMESSA` VARCHAR(100) NOT NULL,
  `NULOTE` VARCHAR(100) NOT NULL,
  `NUQUANTIDADE` INT NOT NULL,
  `NUVALORUNITARIO` DECIMAL(7,2) NOT NULL,
  PRIMARY KEY (`SQESTOQUE`),
  INDEX `fk_FORNECEDOR_has_MATERIAL_MATERIAL1_idx` (`SQMATERIAL` ASC),
  INDEX `fk_FORNECEDOR_has_MATERIAL_FORNECEDOR1_idx` (`SQFORNECEDOR` ASC),
  CONSTRAINT `fk_FORNECEDOR_has_MATERIAL_FORNECEDOR1`
    FOREIGN KEY (`SQFORNECEDOR`)
    REFERENCES `PETS`.`FORNECEDOR` (`SQFORNECEDOR`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_FORNECEDOR_has_MATERIAL_MATERIAL1`
    FOREIGN KEY (`SQMATERIAL`)
    REFERENCES `PETS`.`MATERIAL` (`SQMATERIAL`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`ESPECIEANIMAL`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`ESPECIEANIMAL` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`ESPECIEANIMAL` (
  `SQESPECIE` INT NOT NULL AUTO_INCREMENT,
  `NMNOME` VARCHAR(255) NOT NULL,
  `SGESPECIE` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`SQESPECIE`),
  UNIQUE INDEX `SGESPECIE_UNIQUE` (`SGESPECIE` ASC))
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`RACA`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`RACA` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`RACA` (
  `SQRACA` INT NOT NULL AUTO_INCREMENT,
  `NMNOME` VARCHAR(100) NOT NULL,
  `SGRACA` VARCHAR(10) NOT NULL,
  `SQESPECIE` INT NOT NULL,
  PRIMARY KEY (`SQRACA`),
  UNIQUE INDEX `SGRACA_UNIQUE` (`SGRACA` ASC),
  INDEX `fk_RACA_ESPECIEANIMAL1_idx` (`SQESPECIE` ASC),
  CONSTRAINT `fk_RACA_ESPECIEANIMAL1`
    FOREIGN KEY (`SQESPECIE`)
    REFERENCES `PETS`.`ESPECIEANIMAL` (`SQESPECIE`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`SEXOANIMAL`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`SEXOANIMAL` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`SEXOANIMAL` (
  `SQSEXOANIMAL` INT NOT NULL AUTO_INCREMENT,
  `NMNOME` VARCHAR(100) NOT NULL,
  `SGSEXOANIMAL` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`SQSEXOANIMAL`),
  UNIQUE INDEX `SGSEXOANIMAL_UNIQUE` (`SGSEXOANIMAL` ASC))
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`ANIMAL`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`ANIMAL` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`ANIMAL` (
  `SQANIMAL` INT NOT NULL AUTO_INCREMENT,
  `NMNOME` VARCHAR(500) NOT NULL,
  `DTNASCIMENTO` DATE NOT NULL,
  `SQSEXOANIMAL` INT NOT NULL,
  `SQRACA` INT NOT NULL,
  PRIMARY KEY (`SQANIMAL`),
  INDEX `fk_ANIMAL_SEXOANIMAL1_idx` (`SQSEXOANIMAL` ASC),
  INDEX `fk_ANIMAL_RACA1_idx` (`SQRACA` ASC),
  CONSTRAINT `fk_ANIMAL_SEXOANIMAL1`
    FOREIGN KEY (`SQSEXOANIMAL`)
    REFERENCES `PETS`.`SEXOANIMAL` (`SQSEXOANIMAL`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_ANIMAL_RACA1`
    FOREIGN KEY (`SQRACA`)
    REFERENCES `PETS`.`RACA` (`SQRACA`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`CLIENTEANIMAL`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`CLIENTEANIMAL` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`CLIENTEANIMAL` (
  `SQCLIENTE` INT NOT NULL,
  `SQANIMAL` INT NOT NULL,
  PRIMARY KEY (`SQCLIENTE`, `SQANIMAL`),
  INDEX `fk_CLIENTE_has_ANIMAL_ANIMAL1_idx` (`SQANIMAL` ASC),
  INDEX `fk_CLIENTE_has_ANIMAL_CLIENTE1_idx` (`SQCLIENTE` ASC),
  CONSTRAINT `fk_CLIENTE_has_ANIMAL_CLIENTE1`
    FOREIGN KEY (`SQCLIENTE`)
    REFERENCES `PETS`.`CLIENTE` (`SQCLIENTE`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_CLIENTE_has_ANIMAL_ANIMAL1`
    FOREIGN KEY (`SQANIMAL`)
    REFERENCES `PETS`.`ANIMAL` (`SQANIMAL`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`STATUSAGENDA`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`STATUSAGENDA` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`STATUSAGENDA` (
  `SQSTATUSAGENDA` INT NOT NULL AUTO_INCREMENT,
  `NMNOME` VARCHAR(45) NOT NULL,
  `SGSTATUS` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`SQSTATUSAGENDA`),
  UNIQUE INDEX `SGSTATUS_UNIQUE` (`SGSTATUS` ASC))
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`SERVICO`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`SERVICO` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`SERVICO` (
  `SQSERVICO` INT NOT NULL AUTO_INCREMENT,
  `NMNOME` VARCHAR(255) NOT NULL,
  `SGSERVICO` VARCHAR(10) NOT NULL,
  `VLVALOR` DECIMAL(7,2) NOT NULL,
  PRIMARY KEY (`SQSERVICO`),
  UNIQUE INDEX `SGTIPO_UNIQUE` (`SGSERVICO` ASC))
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`AGENDA`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`AGENDA` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`AGENDA` (
  `SQAGENDA` INT NOT NULL AUTO_INCREMENT,
  `SQCLIENTE` INT NOT NULL,
  `SQANIMAL` INT NOT NULL,
  `SQSTATUSAGENDA` INT NOT NULL,
  `SQSERVICO` INT NOT NULL,
  `SQFUNCIONARIO` INT NOT NULL,
  `DTDATA` DATETIME NOT NULL,
  PRIMARY KEY (`SQAGENDA`),
  INDEX `fk_AGENDA_CLIENTEANIMAL1_idx` (`SQCLIENTE` ASC, `SQANIMAL` ASC),
  INDEX `fk_AGENDA_STATUSAGENDA1_idx` (`SQSTATUSAGENDA` ASC),
  INDEX `fk_AGENDA_TIPOAGENDA1_idx` (`SQSERVICO` ASC),
  INDEX `fk_AGENDA_FUNCIONARIO1_idx` (`SQFUNCIONARIO` ASC),
  CONSTRAINT `fk_AGENDA_CLIENTEANIMAL1`
    FOREIGN KEY (`SQCLIENTE` , `SQANIMAL`)
    REFERENCES `PETS`.`CLIENTEANIMAL` (`SQCLIENTE` , `SQANIMAL`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_AGENDA_STATUSAGENDA1`
    FOREIGN KEY (`SQSTATUSAGENDA`)
    REFERENCES `PETS`.`STATUSAGENDA` (`SQSTATUSAGENDA`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_AGENDA_TIPOAGENDA1`
    FOREIGN KEY (`SQSERVICO`)
    REFERENCES `PETS`.`SERVICO` (`SQSERVICO`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_AGENDA_FUNCIONARIO1`
    FOREIGN KEY (`SQFUNCIONARIO`)
    REFERENCES `PETS`.`FUNCIONARIO` (`SQFUNCIONARIO`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`ATENDIMENTO`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`ATENDIMENTO` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`ATENDIMENTO` (
  `SQATENDIMENTO` INT UNSIGNED NOT NULL,
  `DSOBSERVACAO` VARCHAR(500) NULL,
  `SQAGENDA` INT NULL,
  `DTINICIAL` DATETIME NULL,
  `DTFINAL` DATETIME NULL,
  `SQATENDIMENTOPAI` INT NULL,
  PRIMARY KEY (`SQATENDIMENTO`),
  INDEX `fk_ATENDIMENTO_AGENDA1_idx` (`SQAGENDA` ASC),
  INDEX `fk_ATENDIMENTO_ATENDIMENTO1_idx` (`SQATENDIMENTOPAI` ASC),
  CONSTRAINT `fk_ATENDIMENTO_AGENDA1`
    FOREIGN KEY (`SQAGENDA`)
    REFERENCES `PETS`.`AGENDA` (`SQAGENDA`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_ATENDIMENTO_ATENDIMENTO1`
    FOREIGN KEY (`SQATENDIMENTOPAI`)
    REFERENCES `PETS`.`ATENDIMENTO` (`SQATENDIMENTO`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`CAIXA`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`CAIXA` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`CAIXA` (
  `SQCAIXA` INT NOT NULL AUTO_INCREMENT,
  `CLIENTE_SQCLIENTE` INT NOT NULL,
  `FUNCIONARIO_SQFUNCIONARIO` INT NOT NULL,
  PRIMARY KEY (`SQCAIXA`),
  INDEX `fk_CAIXA_CLIENTE1_idx` (`CLIENTE_SQCLIENTE` ASC),
  INDEX `fk_CAIXA_FUNCIONARIO1_idx` (`FUNCIONARIO_SQFUNCIONARIO` ASC),
  CONSTRAINT `fk_CAIXA_CLIENTE1`
    FOREIGN KEY (`CLIENTE_SQCLIENTE`)
    REFERENCES `PETS`.`CLIENTE` (`SQCLIENTE`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_CAIXA_FUNCIONARIO1`
    FOREIGN KEY (`FUNCIONARIO_SQFUNCIONARIO`)
    REFERENCES `PETS`.`FUNCIONARIO` (`SQFUNCIONARIO`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`MOVIMENTACAO`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`MOVIMENTACAO` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`MOVIMENTACAO` (
  `SQMOVIMENTACAO` INT NOT NULL AUTO_INCREMENT,
  `SQSERVICO` INT NULL,
  `SQESTOQUE` INT NULL,
  `QTQUANTIDADE` INT NULL,
  `VLVALOR` DECIMAL(7,2) NOT NULL,
  `SQCAIXA` INT NOT NULL,
  `VLDESCONTO` INT NULL,
  PRIMARY KEY (`SQMOVIMENTACAO`),
  INDEX `fk_MOVIMENTACAO_SERVICO1_idx` (`SQSERVICO` ASC),
  INDEX `fk_MOVIMENTACAO_ESTOQUE1_idx` (`SQESTOQUE` ASC),
  INDEX `fk_MOVIMENTACAO_CAIXA1_idx` (`SQCAIXA` ASC),
  CONSTRAINT `fk_MOVIMENTACAO_SERVICO1`
    FOREIGN KEY (`SQSERVICO`)
    REFERENCES `PETS`.`SERVICO` (`SQSERVICO`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_MOVIMENTACAO_ESTOQUE1`
    FOREIGN KEY (`SQESTOQUE`)
    REFERENCES `PETS`.`ESTOQUE` (`SQESTOQUE`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_MOVIMENTACAO_CAIXA1`
    FOREIGN KEY (`SQCAIXA`)
    REFERENCES `PETS`.`CAIXA` (`SQCAIXA`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`FORMAPAGAMENTO`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`FORMAPAGAMENTO` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`FORMAPAGAMENTO` (
  `SQFORMAPAGAMENTO` INT NOT NULL AUTO_INCREMENT,
  `NMNOME` VARCHAR(100) NULL,
  `SGFORMAPAGAMENTO` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`SQFORMAPAGAMENTO`),
  UNIQUE INDEX `SGFORMAPAGAMENTO_UNIQUE` (`SGFORMAPAGAMENTO` ASC))
ENGINE = InnoDB;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `PETS`.`PAGAMENTO`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PETS`.`PAGAMENTO` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `PETS`.`PAGAMENTO` (
  `SQCAIXA` INT NOT NULL,
  `SQFORMAPAGAMENTO` INT NOT NULL,
  `VLVALORTOTAL` DECIMAL(7,2) NOT NULL,
  PRIMARY KEY (`SQCAIXA`, `SQFORMAPAGAMENTO`),
  INDEX `fk_CAIXA_has_FORMAPAGAMENTO_FORMAPAGAMENTO1_idx` (`SQFORMAPAGAMENTO` ASC),
  INDEX `fk_CAIXA_has_FORMAPAGAMENTO_CAIXA1_idx` (`SQCAIXA` ASC),
  CONSTRAINT `fk_CAIXA_has_FORMAPAGAMENTO_CAIXA1`
    FOREIGN KEY (`SQCAIXA`)
    REFERENCES `PETS`.`CAIXA` (`SQCAIXA`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_CAIXA_has_FORMAPAGAMENTO_FORMAPAGAMENTO1`
    FOREIGN KEY (`SQFORMAPAGAMENTO`)
    REFERENCES `PETS`.`FORMAPAGAMENTO` (`SQFORMAPAGAMENTO`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;