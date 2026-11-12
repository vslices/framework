# Caso de estudio — campaña de actividades comunitarias (iteración 2)

> **Material para el agente evaluado.** Este documento reproduce un levantamiento funcional resumido, no un diseño técnico. No consultar otros archivos dentro de `tests/skills/vslices-spaces-development/`: ese directorio contiene material reservado de evaluación.

## Contexto del encargo

Una organización comunitaria está preparando un sistema para organizar actividades y recibir inscripciones. Hasta ahora llevan parte del trabajo en planillas y quieren evitar errores cuando varias personas gestionan actividades.

Después de conversar con quienes organizan las actividades, se recogieron las siguientes observaciones y acuerdos. No existe aún un diseño del modelo de software.

## Notas del levantamiento

**Sobre las actividades**

«Cuando abrimos una actividad tenemos que darle un nombre y definir los cupos disponibles. No tiene sentido anunciar una actividad para cero personas ni para una cantidad negativa».

«A veces acompañamos el nombre con un texto explicativo. Si no tenemos nada útil que decir, preferimos dejar ese texto sin completar; no queremos guardar descripciones hechas solamente de espacios».

«Nos ha pasado que dos actividades tienen nombres muy parecidos, así que la organización necesita poder reconocer cada actividad de manera inequívoca, aunque luego se le cambie el nombre. El sistema les asignará un identificador propio cuando se registren. No hemos decidido cómo se representará ese identificador ni cómo se generará».

«Después de publicar una actividad podemos corregir su nombre o el texto explicativo. Los cupos comprometidos, en cambio, no se pueden aumentar ni reducir. Tampoco queremos que una actividad pase a ser otra solo por editar sus datos».

**Sobre las inscripciones**

«Una actividad puede publicarse sin personas inscritas. Después van llegando las solicitudes, una a una, y necesitamos saber quién llegó primero».

«A veces alguien vuelve a intentar inscribirse porque olvidó que ya lo había hecho. Eso no debe ocupar otro cupo de la misma actividad, pero esa persona sí puede inscribirse en otra actividad».

«Cuando se completan los cupos, no se reciben más participantes. En esta etapa del proyecto tampoco se contempla retirar personas ya inscritas ni cambiarles el lugar que ocuparon en el orden de llegada: solo se incorporan nuevas inscripciones al final».

«Tenemos datos para distinguir a los participantes, incluso cuando dos comparten nombre. También se les asigna un identificador interno, pero todavía no decidimos su representación o formato. Si alguien ya está inscrito, una edición no debería convertir su inscripción en la de otra persona».

«No todas las inscripciones son hechas por personas individuales. También participan organizaciones de la comunidad. Cuando se trata de una persona, registramos su **nombre de pila**; cuando se trata de una organización, lo que registramos es su **razón social**. Son maneras distintas de identificar cómo presentar al participante, no queremos tratarlas como el mismo dato ni obligar a completar las dos».

«En los formularios actuales, el equipo sabe si está registrando a una persona o a una organización antes de ingresar el nombre correspondiente. No hemos definido límites de longitud, reglas de normalización o qué hacer con apellidos, nombres comerciales u otras variantes. Tampoco se conversó si una inscripción podría pasar de persona a organización después de registrarse».

**Sobre cuentas de otra plataforma**

«Algunas personas tienen una cuenta en una plataforma administrada por otra organización y quieren dejarla asociada a su inscripción. Otras no tienen ninguna, y eso está bien».

«Nosotros no administramos esas cuentas. Todavía no nos entregan la documentación que explica cómo se identifican o consultan desde nuestro sistema. Por ahora no necesitamos comunicarnos con esa plataforma».

«No conversamos aún sobre si varias personas pueden asociar una misma cuenta ni sobre otras condiciones de esa relación».

**Sobre encontrar actividades**

«Los encargados quieren encontrar actividades escribiendo las primeras letras del nombre. Por ejemplo, al escribir `ac` esperan ver tanto `ACT 1` como `ACt 2`, y que cada nombre siga apareciendo con la escritura que se registró».

«No hemos conversado qué debería pasar con las tildes, signos, espacios o letras que tengan distintas representaciones. Lo importante por ahora es el comportamiento respecto de mayúsculas y minúsculas».

## Solicitud al equipo de desarrollo

A partir de este levantamiento, preparar el **modelo semántico de Spaces** para el módulo. Utilizar la Skill `vslices-spaces-development` disponible en el repositorio [vslices/framework](https://github.com/vslices/framework), consultando sus referencias y los contratos efectivos del Framework.

Implementar los Spaces pertinentes con construcción y, cuando el dominio lo necesite, evolución. Preparar pruebas que permitan verificar tanto casos admitidos como rechazos relevantes. Limitar esta iteración al modelo semántico; no se solicita construir interfaces, Features completas, persistencia ni integración con la plataforma externa.

Entregar los archivos o el código completo que se produzca, distinguir las verificaciones realmente ejecutadas de las pendientes y cerrar con el informe de modelado que solicita la Skill.

No completar por intuición las decisiones que no surgieron del levantamiento. Cuando falte información, registrar qué falta y qué implicaciones tiene para el modelo.
