# Agent-PC (HeyClicky Reverse Engineered & Open Source)

Base de code C# .NET 9 entièrement décompilée, analysée et prête pour la personnalisation complète (modèle LLM, pipeline vocal local, serveur d'endpoints personnalisable).

---

## Architecture de la Solution

| Projet | Rôle & Description |
|---|---|
| **`HeyClicky`** | Point d'entrée WinUI 3 (Windows App SDK), gestion du cycle de vie de l'application, barres de tâches, fenêtres d'onboarding et UI. |
| **`HeyClicky.Core`** | Cœur applicatif : pipeline audio (STT via Deepgram, TTS via OpenAI Realtime), sessions interactives (`CompanionSession`), réseau HTTP/WebSocket. |
| **`HeyClicky.Agents`** | Gestionnaire d'agents (`AgentService`), intégration du runtime Codex (`codex.exe`) et du runner de tâche (`clicky-harness.exe`). |
| **`HeyClicky.Drawing`** | Moteur de rendu vectoriel et DirectX / Direct2D pour les overlays d'écran, dessins et animations de pointage. |
| **`HeyClicky.Platform`** | Intégration bas niveau Windows : capture microphone WASAPI 24 kHz, capture d'écran, gestionnaires d'événements et fenêtres Win32. |
| **`HeyClicky.Contracts`** | Définitions de contrats, DTOs de communication, événements et configuration réseau (`ClickyEndpoints`). |

---

## Fonctionnalités Clés & Personnalisation

### 1. Remplacement du Pipeline Vocal (STT / TTS)

#### Entrée Vocale (STT - Speech to Text)
- Situé dans `HeyClicky.Core/HeyClicky/Core/Voice/DeepgramTranscriber.cs`.
- Capture en **24 000 Hz PCM 16-bit Mono** (`linear16`).
- Peut être remplacé par un serveur local **Faster-Whisper** ou **Whisper.net**.
- Dispositif de secours intégré : envoi d'un WAV en Base64 via `CompanionSession.TranscribeFallbackAsync`.

#### Sortie Vocale (TTS - Text to Speech)
- Situé dans `HeyClicky.Core/HeyClicky/Core/Voice/RealtimeMouth.cs`.
- Reçoit des flux de données audio PCM 16-bit réinjectés dans `IAudioOutput`.
- La synchronisation du tracé sur l'écran et des sous-titres est asservie sur `audio.PlayedSeconds`.
- Peut être substitué par **Piper-TTS**, **Kokoro**, ou **XTTS**.

---

### 2. Redirection des Endpoints (Sans recompiler)

L'application prend nativement en charge la surcharge des endpoints via `endpoints.json` :
1. Créez `%LOCALAPPDATA%\HeyClicky\endpoints.json` :
```json
{
  "workerBaseUrl": "http://127.0.0.1:8787",
  "supabaseUrl": "http://127.0.0.1:54321",
  "supabaseAnonKey": "votre-cle-locale"
}
```
2. Ou définissez la variable d'environnement :
```powershell
$env:HEYCLICKY_WORKER_BASE_URL="http://127.0.0.1:8787"
```

---

### 3. Personnalisation du Modèle LLM de l'Agent

Le moteur d'agent Codex est configuré via :
`%LOCALAPPDATA%\HeyClicky\codex-home\config.toml`

Vous pouvez rediriger les requêtes vers n'importe quel serveur compatible OpenAI (Ollama, vLLM, LM Studio, etc.) :
```toml
[model_providers.clicky]
name = "Local"
base_url = "http://127.0.0.1:11434/v1" # Ollama par exemple
```

---

## Compilation

Prérequis :
- .NET 9 SDK
- Windows 10/11 x64 (Build 19041+)

```bash
dotnet build HeyClicky/HeyClicky.csproj -c Release
```
