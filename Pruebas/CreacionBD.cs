using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DST
{
    class CreacionBD
    {
        public CreacionBD()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.UserID = "root";
            builder.Password = "";
            builder.Database = "bddst";

            MySqlConnection conn = new MySqlConnection(builder.ToString());
            MySqlCommand cmd = conn.CreateCommand();
            //cmd.CommandText = "INSERT INTO mitabla (valor1,valor2) value (1,2)";

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS empresa("
            + "nombre text,"
            + "razonSocial varchar(12),"
            + "direccion text,"
            + "primary key(razonSocial)"
            + ");";
            conn.Open();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS secciones("
            + "id MEDIUMINT NOT NULL AUTO_INCREMENT,"
            + "nombre text,"
            + "descripcion text,"
            + "rutJefe varchar(12),"
            + "primary key(id),"
            + "foreign key(rutJefe) references usuarios(rut)"
            + "on delete no action on update no action"
            + ");";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS usuarios("
            + "nombre text,"
            + "rut varchar(12),"
            + "clave varchar (20),"
            + "tipoUsuario varchar(30),"
            + "estado boolean,"
            + "primary key(rut)"
            + ");";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS trabajadores("
            + "nombre text,"
            + "rut varchar(12),"
            + "fechaNacimiento date,"
            + "idSeccion MEDIUMINT NOT NULL,"
            + "estado boolean,"
            + "primary key(rut),"
            + "foreign key(idSeccion)references secciones(id)"
            + "on delete no action on update no action"
            + ");";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS componentesPerfil("
            + "nombre varchar (100),"
            + "descripcion text,"
            + "tipo varchar(20),"
            + "estado boolean,"
            + "primary key(nombre)"
            + ");";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS componentesPerfilSecciones("
            + "idSeccion MEDIUMINT NOT NULL,"
            + "nombre varchar(100),"
            + "puntaje float,"
            + "importancia float,"
            + "primary key(idSeccion, nombre),"
            + "foreign key(idSeccion)references secciones(id)"
            + "on delete no action on update no action,"
            + "foreign key(nombre) references componentesPerfil(nombre)"
            + "on delete no action on update no action"
            + ");";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS componentesPerfilTrabajadores("
            + "rut varchar(12),"
            + "nombre varchar(100),"
            + "puntaje float,"
            + "primary key(rut, nombre),"
            + "foreign key(rut) references trabajadores(rut)"
            + "on delete no action on update no action,"
            + "foreign key(nombre) references componentesPerfil(nombre)"
            + "on delete no action on update no action"
            + ");";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS desempeño("
            + "idSeccion MEDIUMINT NOT NULL,"
            + "fecha date,"
            + "ventasAñoActual float,"
            + "ventasAñoAnterior float,"
            + "ventasPlan float,"
            + "reubicaciones integer,"
            + "totalEmpleados integer,"
            + "empleadosConAdvertencia integer,"
            + "primary key(idSeccion, fecha),"
            + "foreign key(idSeccion) references secciones(id)"
            + "on delete no action on update no action"
            + ");";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS reubicaciones("
            + "rut varchar(12),"
            + "idSeccionAnterior MEDIUMINT NOT NULL,"
            + "idSeccionNueva MEDIUMINT NOT NULL,"
            + "fecha date,"
            + "primary key(idSeccionAnterior, idSeccionNueva, fecha),"
            + "foreign key(rut) references trabajadores(rut)"
            + "on delete no action on update no action,"
            + "foreign key(idSeccionAnterior) references secciones(id)"
            + "on delete no action on update no action,"
            + "foreign key(idSeccionNueva) references secciones(id)"
            + "on delete no action on update no action"
            + ");";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS preguntasComponentes("
            + "id MEDIUMINT NOT NULL AUTO_INCREMENT,"
            + "nombre varchar (100),"
            + "pregunta text,"
            + "estado boolean,"
            + "primary key(id),"
            + "foreign key(nombre) references componentesPerfil(nombre)"
            + "on delete no action on update no action"
            + ");";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS alternativasPreguntasComponentes("
            + "idPregunta MEDIUMINT NOT NULL,"
            + "alternativa varchar(100),"
            + "primary key(idPregunta, alternativa),"
            + "foreign key(idPregunta) references preguntasComponentes(id)"
            + "on delete no action on update no action"
            + ");";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS regla("
            + "id MEDIUMINT NOT NULL AUTO_INCREMENT,"
            + "operador varchar (20),"
            + "nombreVariable varchar(100),"
            + "nombreValor varchar(100),"
            + "tipoVariable varchar(20),"
            + "idSeccion  MEDIUMINT NOT NULL,"
            + "tipoComponente varchar(20),"
            + "primary key(id),"
            + "foreign key(idSeccion) references secciones(id)"
            + "on delete no action on update no action"
            + ");";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS variablesLing("
            + "idRegla MEDIUMINT NOT NULL,"
            + "nombre varchar(100),"
            + "minimo integer,"
            + "maximo integer,"
            + "tipo varchar (100),"
            + "primary key(nombre, tipo),"
            + "foreign key(idRegla) references regla(id)"
            + "on delete no action on update no action,"
            + "foreign key(nombre) references componentesPerfil(nombre)"
            + "on delete no action on update no action"
            + ");";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS valoresLing("
            + "nombre varchar (100),"
            + "nombreVariableLing varchar(100),"
            + "tipoFuncion varchar(20),"
            + "primary key(nombre, nombreVariableLing),"
            + "foreign key(nombreVariableLing) references componentesPerfil(nombre)"
            + "on delete no action on update no action"
            + ");";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS funcionTrapezoide("
            + "nombreVariableLing varchar (100),"
            + "nombreValorLing varchar(100),"
            + "valorInferiorIzquierdo integer,"
            + "valorSuperiorIzquierdo integer,"
            + "valorSuperiorDerecho integer,"
            + "valorInferiorDerecho integer,"
            + "primary key(nombreVariableLing, nombreValorLing),"
            + "foreign key(nombreVariableLing) references componentesPerfil(nombre)"
            + "on delete no action on update no action,"
            + "foreign key(nombreValorLing) references valoresLing(nombre)"
            + "on delete no action on update no action"
            + ");";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS funcionTriangular("
            + "nombreVariableLing varchar (100),"
            + "nombreValorLing varchar(100),"
            + "valorIzquierda integer,"
            + "valorCentro integer,"
            + "valorDerecha integer,"
            + "primary key(nombreVariableLing, nombreValorLing),"
            + "foreign key(nombreVariableLing) references componentesPerfil(nombre)"
            + "on delete no action on update no action,"
            + "foreign key(nombreValorLing) references valoresLing(nombre)"
            + "on delete no action on update no action"
            + ");";
            cmd.ExecuteNonQuery();
        }
    }
}
