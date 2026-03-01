# Deploying Blazor WebAssembly (.NET 8) to Vercel

# ✅ Step 1 — Install Vercel CLI

```bash
npm i -g vercel
```

Check:

```bash
vercel --version
```

---

# ✅ Step 2 — Login

```bash
vercel login
```

Choose:

```
Continue with Email
```

Verify from your email.

---

# ✅ Step 3 — Publish .NET 8 Blazor WASM

Go to your project root (where `.csproj` exists):

```bash
dotnet publish -c Release
```

After build completes, output folder will be:

```
bin/Release/net8.0/publish/wwwroot
```

⚠ Make sure it says `net8.0`

---

# ✅ Step 4 — Deploy (Simple & Recommended Way)

Instead of configuring build in Vercel, just deploy the static output:

```bash
cd bin/Release/net8.0/publish/wwwroot
vercel --prod
```

That’s it ✅

Vercel will treat it as a static site.

---

# ✅ Step 5 — Fix SPA Routing (IMPORTANT)

If you refresh a page and get 404, create `vercel.json` inside the `wwwroot` folder before deploying:

```json
{
  "routes": [
    { "handle": "filesystem" },
    { "src": "/.*", "dest": "/index.html" }
  ]
}
```

Then redeploy:

```bash
vercel --prod
```

---

# 🚀 Full Command Flow (.NET 8)

```bash
npm i -g vercel
vercel login

dotnet publish -c Release
cd bin/Release/net8.0/publish/wwwroot

vercel --prod
```

---

# 🔥 Alternative (Project Root Deployment)

If you want deploy from root instead:

Create `vercel.json` in project root:

```json
{
  "buildCommand": "dotnet publish -c Release",
  "outputDirectory": "bin/Release/net8.0/publish/wwwroot"
}
```

Then:

```bash
vercel --prod
```

---

# ⚠ Important

If your project is:

- ✔ Blazor WebAssembly Standalone → Works perfectly
- ❌ Blazor Web App (Server or Auto render mode) → NOT supported on Vercel

Vercel only supports static hosting.

---