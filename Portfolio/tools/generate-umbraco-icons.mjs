// Extracts every icon of the Umbraco backoffice icon picker into Resources/umbraco-icons.json.
// Usage: node tools/generate-umbraco-icons.mjs [path to Umbraco.Cms.StaticAssets backoffice folder]
// Re-run after upgrading Umbraco so new icons become available on the front-end.
import { readdirSync, writeFileSync, mkdirSync } from 'node:fs';
import { homedir } from 'node:os';
import { join, dirname } from 'node:path';
import { pathToFileURL, fileURLToPath } from 'node:url';

const here = dirname(fileURLToPath(import.meta.url));

function findBackoffice() {
    if (process.argv[2]) return process.argv[2];

    const root = join(homedir(), '.nuget', 'packages', 'umbraco.cms.staticassets');
    const version = readdirSync(root).sort((a, b) => a.localeCompare(b, undefined, { numeric: true })).at(-1);
    return join(root, version, 'staticwebassets', 'umbraco', 'backoffice');
}

const coreFolder = join(findBackoffice(), 'packages', 'core');
const registryFile = readdirSync(coreFolder).find((file) => /^icons-.+\.js$/.test(file));
const registry = (await import(pathToFileURL(join(coreFolder, registryFile)).href)).default;

const icons = {};
for (const entry of registry) {
    const module = await entry.path();
    const svg = typeof module === 'string' ? module : module.default;

    // Compact the markup: the helper adds its own class / aria attributes.
    icons[entry.name] = svg
        .replace(/\sxmlns="[^"]*"/, '')
        .replace(/\sclass="[^"]*"/, '')
        .trim();
}

const sorted = Object.fromEntries(Object.entries(icons).sort(([a], [b]) => a.localeCompare(b)));
const output = join(here, '..', 'Resources', 'umbraco-icons.json');
mkdirSync(dirname(output), { recursive: true });
writeFileSync(output, JSON.stringify(sorted));
console.log(`${Object.keys(sorted).length} icons written to ${output}`);
