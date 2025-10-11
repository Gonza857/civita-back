## Como actualizar base de datos

### Crear migracion
```
  Add-Migration nombreMigracion
```
Creamos la migración para luego pasarla a leer en la base de datos.

### Actualizamos la base de datos
```
  Update-Database
```
Se actualiza la base de datos de acuerdo a las migraciones

## Cuando alguien actualiza el modelado de entidades (BO, Business Object)
Tenemos que actualizar esos cambios, para ello debemos ejecutar los comandos de antes desde una terminal. La misma se encuentra en

- Barra superior de VS 2022 (Archivo, Editar, Ver, etc)
- Seleccionamos "Ver"
- Buscamos la opcion "Otras ventanas"
- Seleccionamos "Consola del Administrador de Paquetes"
