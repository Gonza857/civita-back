-- Debemos vaciar los logros ya que la columna era nulleable y ahora no lo es
delete from "Logro" where "Id" is not null;
