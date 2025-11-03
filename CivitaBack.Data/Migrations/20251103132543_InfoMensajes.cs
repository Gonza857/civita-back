using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivitaBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class InfoMensajes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""TipoTip""
                SET ""Descripcion"" = CASE ""Id""
                    WHEN 1 THEN 'onboarding'
                    WHEN 2 THEN 'info'
                END
                WHERE ""Id"" IN (1, 2);
            ");

            migrationBuilder.InsertData(
             table: "TipoTip",
             columns: new[] { "Descripcion", "Creado" },
             values: new object[] { "Misión", DateTime.UtcNow }
             );


            migrationBuilder.InsertData(
             table: "Tip",
             columns: new[] { "Mensaje", "TipoId", "EfectoFiltro", "ElementoAdicional", "Expresion", "Creado" },
             values: new object[,]
             {
                    {  "¿Sabías que el sol envía en una hora más energía de la que usamos en todo un año? ☀️⚡", 2, false, "", "vitaPresumida",  DateTime.UtcNow },
                    {  "¿Sabías que el aire contaminado acorta la vida promedio hasta en dos años? 🌫️💨", 2, false, "", "vitaPresumida",  DateTime.UtcNow },
                    {  "¿Sabías que cada colilla de cigarrillo contamina 50 litros de agua? 🚬💧", 2, false, "", "vitaPresumida",  DateTime.UtcNow },
                    {  "¿Sabías que los incendios forestales liberan tanto CO₂ como millones de autos encendidos? 🔥🚗", 2, false, "", "vitaPresumida", DateTime.UtcNow },
                    {  "¿Sabías que el 90% del plástico del mundo no se recicla? 🧃🌊", 2, false, "", "vitaPresumida", DateTime.UtcNow },
                    {  "¿Sabías que los árboles urbanos pueden bajar la temperatura de una ciudad hasta 5 °C? 🌳☀️", 2, false, "", "vitaPresumida", DateTime.UtcNow },
                    {  "¿Sabías que los océanos absorben un cuarto del CO₂ que generamos? 🌊💨", 2, false, "", "vitaPresumida", DateTime.UtcNow },
                    {  "¿Sabías que usar una bicicleta en vez de un auto por un mes puede ahorrar 90 kg de CO₂? 🚴‍♂️💚", 2, false, "", "vitaPresumida", DateTime.UtcNow },
                    {  "¿Sabías que comer local evita que los alimentos viajen miles de kilómetros? 🚜🍎", 2, false, "", "vitaPresumida",  DateTime.UtcNow },
                    { "Plantar un árbol es invertir en futuro. Un solo árbol absorbe 20 kg de CO₂ al año y refresca el aire de tres personas. 🌳💚", 2, false, "", "vitaExplica4", DateTime.UtcNow },
                    { "Los techos verdes bajan el calor y embellecen la ciudad. ¡La naturaleza también puede vivir sobre el cemento! 🏙️🌿", 2, false, "", "vitaExplica4", DateTime.UtcNow },
                    { "Hacer compost transforma residuos en vida. Las cáscaras de frutas se convierten en alimento para nuevas plantas. 🍌🪱", 2, false, "", "vitaExplica4", DateTime.UtcNow },
                    { "Comer más frutas y verduras es bueno para vos y para el planeta. Producir vegetales usa menos agua y energía que criar animales. 🥦🌍", 2, false, "", "vitaExplica4", DateTime.UtcNow },
                    { "Reducir la carne, aunque sea un día a la semana, tiene impacto real. Producir 1 kilo de carne usa 15.000 litros de agua y genera 27 kg de CO₂. 🍔💧", 2, false, "", "vitaExplica4", DateTime.UtcNow },
                    { "Elegir productos locales reduce la contaminación del transporte. Los alimentos que vienen de lejos viajan miles de kilómetros. 🚚🍎", 2, false, "", "vitaExplica4", DateTime.UtcNow },
                    { "Compartir o donar lo que ya no usás evita desperdicio. Lo que para vos sobra, puede servirle a alguien más. ♻️💚", 2, false, "", "vitaExplica4", DateTime.UtcNow },
                    { "Usar la tecnología con conciencia también es ecológico. Internet y los servidores del mundo consumen 3% de toda la energía global. 💻⚡ Apagar pantallas que no usás es un gesto poderoso.", 2, false, "", "vitaExplica4", DateTime.UtcNow },
                    { "Informarte y enseñar a otros genera cambio. Cada conversación puede inspirar a alguien a cuidar el planeta. 🗣️🌎", 2, false, "", "vitaExplica4", DateTime.UtcNow },
                    { "Pequeñas acciones, grandes resultados. Si cada persona cambia un hábito, el impacto global sería gigante. 🌱✨", 2, false, "", "vitaExplica4", DateTime.UtcNow },
                    { "Separar los residuos ayuda al planeta. Cuando mezclamos todo, los reciclables se pierden y terminan en basurales o ríos. ♻️ Dato: separar en casa puede reducir hasta un 40% de la basura que generás.", 2, false, "", "vitaNormal", DateTime.UtcNow },
                    { "Usar una botella reutilizable es un pequeño gran cambio. Una sola botella plástica puede tardar 450 años en desaparecer. 🌊 Imaginate si millones de personas dejaran de usarlas una semana.", 2, false, "", "vitaNormal", DateTime.UtcNow },
                    { "Comprar menos, elegir mejor. La moda rápida contamina tanto como todos los vuelos del planeta juntos. 👕✈️ Apostá por ropa que dure o de segunda mano.", 2, false, "", "vitaNormal", DateTime.UtcNow },
                    { "Limpiar con productos naturales también cuida el agua. Los químicos domésticos llegan a ríos y lagos. 🧴💧 Usar vinagre o bicarbonato puede ser igual de efectivo.", 2, false, "", "vitaNormal", DateTime.UtcNow },
                    { "Apagar las luces tiene más impacto del que creés. Una lámpara encendida sin uso gasta energía que podría alimentar cientos de celulares. 💡📱", 2, false, "", "vitaNormal", DateTime.UtcNow },
                    { "Desenchufar cargadores o consolas también cuenta. Aunque no carguen nada, siguen consumiendo energía. ⚡ Es lo que se llama energía fantasma.", 2, false, "", "vitaNormal", DateTime.UtcNow },
                    { "La luz del sol es la mejor energía renovable. Abrir las cortinas antes que prender la lámpara ahorra electricidad y te conecta con el día. ☀️", 2, false, "", "vitaNormal", DateTime.UtcNow },
                    { "Los focos LED son aliados verdes. Usan 80% menos energía y duran mucho más. Además, reducen residuos electrónicos. 💡🌱", 2, false, "", "vitaNormal", DateTime.UtcNow },
                    { "Caminar, pedalear o compartir el viaje mejora tu salud y el aire. 🚴‍♀️💨 Un auto emite 4,6 toneladas de CO₂ por año, pero si lo usás menos días, ayudás mucho más de lo que creés.", 2, false, "", "vitaNormal", DateTime.UtcNow },
                    { "Usar transporte público es una forma de cooperar con el planeta. Menos autos = menos humo, menos ruido, menos estrés. 🚌🌍", 2, false, "", "vitaNormal", DateTime.UtcNow },
                    { "Cerrar la canilla mientras te lavás los dientes ahorra litros y litros. Hasta 12 litros por minuto. 🚰 Lo que gastás en un día puede ser lo que otra persona necesita en una semana.", 2, false, "", "vitaNormal", DateTime.UtcNow },
                    { "Las duchas cortas también son un acto ecológico. Una de 5 minutos gasta la mitad del agua que una de 10. 🚿💧 En un año, eso puede ahorrar 27.000 litros.", 2, false, "", "vitaNormal", DateTime.UtcNow },
                    { "El agua de lluvia sirve para limpiar o regar. Usarla ayuda a cuidar las reservas de agua potable. 🌧️🌿", 2, false, "", "vitaNormal", DateTime.UtcNow }
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""TipoTip""
                SET ""Descripcion"" = CASE ""Id""
                    WHEN 1 THEN 'info'
                    WHEN 2 THEN 'onboarding'
                END
                WHERE ""Id"" IN (1, 2);
            ");
        }
    }
}
