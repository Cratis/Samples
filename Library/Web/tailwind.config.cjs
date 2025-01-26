/** @type {import('tailwindcss').Config} */
module.exports = {
    content: {
        files: [
            "./index.html",
            "./Components/**/*.tsx",
            "./Features/**/*.tsx",
            "./Layout/**/*.tsx",
            "./node_modules/primereact/**/*.{js,ts,jsx,tsx}"
        ]
    },
    theme: {
        extend: {},
    },
    plugins: [],
}
