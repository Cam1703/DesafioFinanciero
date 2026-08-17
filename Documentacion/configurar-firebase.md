# Configurar Firebase

El juego guarda usuarios, salones y progreso en **Cloud Firestore**. La configuración
del proyecto (`projectId` y `webApiKey`) no está en el repositorio: cada quien la
completa localmente.

## Pasos

1. Copia la plantilla y renómbrala quitando `.example`:

   - `Assets/Resources/firebase-config.json` (usada por el build; **es la que lee el juego primero**)
   - o `Assets/StreamingAssets/firebase-config.json` (alternativa de respaldo)

   `FirebaseConfig.cs` busca primero en `Resources/` y, si no la encuentra, en `StreamingAssets/`.
   Para builds de WebGL es obligatorio usar `Resources/`, porque ahí no se puede leer
   `StreamingAssets` con `File.ReadAllText`.

2. Completa los dos valores desde la consola de Firebase, en
   **⚙️ Configuración del proyecto → General**:

   ```json
   {
     "projectId": "tu-project-id",
     "webApiKey": "tu-web-api-key"
   }
   ```

3. En la consola de Firebase, verifica que estén activos:
   - **Firestore Database** creado (modo producción).
   - **Authentication → Sign-in method → Anonymous** habilitado.
   - Las **Reglas de seguridad** publicadas:

     ```
     rules_version = '2';
     service cloud.firestore {
       match /databases/{database}/documents {
         match /usuarios/{usuarioId} {
           allow read, write: if request.auth != null;
         }
         match /salones/{salonId} {
           allow read, write: if request.auth != null;
         }
       }
     }
     ```

## Sobre la Web API Key

No es un secreto en el sentido tradicional: Firebase la expone en cualquier app cliente
y su documentación lo indica explícitamente. Quien protege los datos son las **Reglas de
Seguridad de Firestore**, no ocultar la clave.

Aun así el archivo está en `.gitignore` por dos motivos: evita que cada quien pise la
configuración del otro al trabajar contra proyectos de Firebase distintos, y mantiene el
repositorio sin credenciales concretas de un proyecto.

**Limitación conocida:** con autenticación anónima, las reglas solo verifican que el
cliente esté autenticado, no que sea el dueño del documento. Para un despliegue más
allá de un aula controlada, habría que vincular el UID anónimo al documento de usuario
y escribir reglas por dueño.
