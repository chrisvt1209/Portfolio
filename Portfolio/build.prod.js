import { build } from 'esbuild';
import { sassPlugin } from 'esbuild-sass-plugin';

await build({
    entryPoints: ['wwwroot/assets/js/**/*.js', 'wwwroot/assets/scss/**/*.scss'],
    outdir: 'wwwroot/bundles/',
    bundle: true,
    minify: true,
    sourcemap: false,
    splitting: true,
    keepNames: true,
    platform: 'browser',
    metafile: true,
    treeShaking: true,
    logLevel: 'info',
    format: 'esm',
    plugins: [sassPlugin()],
    loader: {
        '.eot': 'file',
        '.woff': 'file',
        '.woff2': 'file',
        '.svg': 'file',
        '.ttf': 'file',
        '.png': 'file',
        '.jpg': 'file',
        '.webp': 'file',
        '.json': 'file',
    },
}).catch((err) => console.error(err));

