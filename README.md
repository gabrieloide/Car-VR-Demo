Car VR Demo
Una breve descripción de tu proyecto. ¿Qué es? ¿Qué hace? Explica aquí el propósito principal de la demo. Por ejemplo: "Una experiencia de realidad virtual inmersiva que permite al usuario explorar el interior y exterior de un coche detallado".

Tabla de Contenidos
Requisitos Previos
Instalación
Cómo Ejecutar el Proyecto
Controles
Estructura del Proyecto
Créditos
Requisitos Previos
Para poder abrir y ejecutar este proyecto, necesitarás instalar el siguiente software:

Git: Para clonar el repositorio. Puedes descargarlo desde git-scm.com.

Unity Hub: Para gestionar las versiones de Unity y los proyectos. Descárgalo desde la página oficial de Unity.

Unity Editor: La versión específica de Unity con la que se desarrolló este proyecto.

Versión de Unity requerida: [AQUÍ VA LA VERSIÓN DE UNITY]
Nota: Puedes encontrar la versión exacta en el archivo ProjectSettings/ProjectVersion.txt de este repositorio. Búsca la línea que dice m_EditorVersion:.

SDK de VR (Opcional): Si el proyecto utiliza un SDK de VR específico, indícalo aquí.

Ejemplo para Oculus/Meta: Oculus Integration SDK
Ejemplo para SteamVR: SteamVR Plugin
Nota: Si los paquetes de VR se gestionan a través del Package Manager de Unity, no se necesita instalación manual, pero es bueno mencionarlos.

Instalación
Sigue estos pasos para configurar el proyecto en tu ordenador:

Clona el repositorio: Abre una terminal o Git Bash y ejecuta el siguiente comando en la carpeta donde quieras guardar el proyecto:

Bash

git clone https://docs.github.com/es/repositories
Abre el proyecto en Unity Hub:

Abre Unity Hub.
Haz clic en Open > Add project from disk.
Navega hasta la carpeta que acabas de clonar y selecciónala.
Instala la versión correcta de Unity:

Unity Hub detectará la versión requerida. Si no la tienes instalada, te pedirá que la descargues e instales. Asegúrate de incluir los módulos de Android Build Support o PC, Mac & Linux Standalone Build Support, dependiendo de tu plataforma de destino.
Abre el proyecto:

Una vez instalada la versión correcta, haz clic en el nombre del proyecto en Unity Hub para abrirlo en el Editor de Unity.
La primera vez que lo abras, Unity tardará un tiempo en importar todos los assets. Sé paciente.
Cómo Ejecutar el Proyecto
Una vez que el proyecto esté abierto en el Editor de Unity:

Abre la escena principal:

En la ventana Project (normalmente abajo), navega a la carpeta Assets/Scenes/.
Busca la escena principal (por ejemplo, MainScene.unity o CarDemo.unity) y haz doble clic sobre ella para abrirla.
Ejecuta la demo:

Asegúrate de que tu dispositivo de VR esté conectado y funcionando correctamente (si aplica).
Haz clic en el botón de Play (▶) en la parte superior central del Editor de Unity.
Para compilar (Build):

Ve a File > Build Settings.
Selecciona tu plataforma de destino (por ejemplo, PC, Mac & Linux Standalone o Android).
Haz clic en Build y elige una carpeta donde guardar el ejecutable.
Controles
Describe aquí los controles para interactuar con la demo.

Acción	Botón / Control
Moverse / Teletransportarse	[Ej: Gatillo derecho]
Interactuar con puertas	[Ej: Botón A]
Cambiar de vista	[Ej: Joystick izquierdo]
... (añade más acciones)	...

Exportar a Hojas de cálculo
Estructura del Proyecto
Una breve descripción de cómo están organizadas las carpetas principales:

/Assets: Contiene todos los assets del juego.
/Scenes: Las escenas del proyecto.
/Scripts: Todos los scripts C#.
/Prefabs: Objetos pre-configurados listos para usar.
/Materials: Materiales y texturas para los modelos 3D.
/Models: Los modelos 3D utilizados.
