# Deploying Study Quest API to Fly.io

## Prerequisites

1. Install the Fly CLI: <https://fly.io/docs/flyctl/install/>
2. Sign up / log in:
   ```bash
   fly auth signup   # or: fly auth login
   ```

---

## 1. Create the Fly App + Postgres Database

From the **`backend/`** folder:

```bash
cd backend

# Create the app (uses fly.toml settings, skips immediate deploy)
fly launch --no-deploy

# Create a managed Postgres cluster (single node, development tier)
fly postgres create --name study-quest-db --region cdg --vm-size shared-cpu-1x --volume-size 1

# Attach the database to the app (auto-sets DATABASE_URL secret)
fly postgres attach study-quest-db --app study-quest-api
```

> The attach command sets a `DATABASE_URL` secret on the app automatically.  
> However, our app reads `ConnectionStrings__DefaultConnection`, so we'll map it below.

---

## 2. Set Secrets

All sensitive configuration is passed via **Fly secrets** (environment variables), never committed to source control.

```bash
# Get the DATABASE_URL that was set by the attach command
fly secrets list --app study-quest-api

# Set the connection string (convert the DATABASE_URL to Npgsql format)
# Example: Host=study-quest-db.flycast;Port=5432;Database=study_quest_api;Username=postgres;Password=<password>
fly secrets set \
  "ConnectionStrings__DefaultConnection=Host=study-quest-db.flycast;Port=5432;Database=study_quest_api;Username=postgres;Password=YOUR_DB_PASSWORD" \
  "JwtSettings__Secret=YOUR-LONG-RANDOM-SECRET-AT-LEAST-32-CHARS" \
  "TwilioSettings__AccountSid=YOUR_TWILIO_SID" \
  "TwilioSettings__AuthToken=YOUR_TWILIO_AUTH" \
  "TwilioSettings__PhoneNumber=+1234567890" \
  "OpenAISettings__ApiKey=YOUR_OPENAI_KEY" \
  --app study-quest-api
```

> Replace the placeholder values with your actual credentials.

---

## 3. Deploy

```bash
fly deploy
```

Fly.io will:
1. Build the Docker image using the existing `Dockerfile`
2. Push it to Fly's registry
3. Start the app on port 8080 behind their TLS edge

---

## 4. Verify

```bash
# Check app status
fly status --app study-quest-api

# View live logs
fly logs --app study-quest-api

# Open the Scalar API docs in your browser
fly open /scalar/v1 --app study-quest-api
```

Your API is now live at: **`https://study-quest-api.fly.dev`**

---

## 5. Expo App Configuration

The API URL has been updated in `src/services/api.ts`:

- **Development** (`__DEV__` = true): uses your local IP (`http://10.19.142.221:5197`)
- **Production** (`__DEV__` = false): uses `https://study-quest-api.fly.dev`

No manual switching needed — React Native's `__DEV__` flag handles it automatically.

---

## Useful Commands

| Command | Description |
| --- | --- |
| `fly deploy` | Deploy latest changes |
| `fly logs` | Stream live logs |
| `fly status` | App health & machine info |
| `fly secrets list` | List configured secrets |
| `fly secrets set KEY=VALUE` | Add/update a secret |
| `fly ssh console` | SSH into the running machine |
| `fly scale count 1` | Scale to 1 always-on machine |
| `fly postgres connect --app study-quest-db` | Connect to the DB via psql |

---

## Troubleshooting

- **App won't start**: Check `fly logs` for startup errors. Common issues:
  - Missing secrets → `fly secrets list` to verify
  - Database not reachable → ensure Postgres is attached
- **502 errors**: The app may be booting. Wait 10-20 seconds for the machine to resume (auto-start is enabled).
- **Migration failures**: The app auto-runs EF Core migrations on startup. Check logs for details.
