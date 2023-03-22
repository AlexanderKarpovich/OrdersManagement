/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
    "./src/*.{js,jsx,ts,tsx}",
    "./src/**/**/**/*.{js,jsx,ts,tsx}",
  ],
  theme: {
    extend: {
      animation: {
        "bounce-200": 'bounce 1s infinite 200ms',
        "bounce-400": 'bounce 1s infinite 400ms'
      }
    },
  },
  plugins: [],
}
