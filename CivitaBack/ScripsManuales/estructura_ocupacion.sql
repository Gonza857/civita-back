-- Ejecutar antes de hacer la migración 18/10/25 21:45 By Gonza
-- Cambiamos la columna ocupacion que debe ser un numero, que estaba en string a integer.
UPDATE "TipoEstructura" SET "Ocupacion" = null where "Id" >= 0;
ALTER TABLE "TipoEstructura"
ALTER COLUMN "Ocupacion" TYPE integer USING "Ocupacion"::integer;

SELECT setval(
    pg_get_serial_sequence('"TipoEstructura"', 'Id'),
    COALESCE(MAX("Id"), 0) + 1,
    false
)
FROM "TipoEstructura";