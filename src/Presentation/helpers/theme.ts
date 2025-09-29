const themeData = {
  storageKey: "color-scheme",
  dark: "dark",
  light: "light",
  media: "(prefers-color-scheme: dark)",
  element: "body"
}

function get() {
  const prefersDark = window.matchMedia(themeData.media).matches;
  const savedTheme = localStorage.getItem(themeData.storageKey);
  return savedTheme || (prefersDark ? themeData.dark : themeData.light);
}

function toggle() {
  const newTheme = get() === themeData.dark ? themeData.light : themeData.dark;
  document.querySelector(themeData.element)!.classList.remove(get());
  document.querySelector(themeData.element)!.classList.add(newTheme);
  localStorage.setItem(themeData.storageKey, newTheme);
}

function apply() {
  const theme = get();
  document.querySelector(themeData.element)!.classList.add(theme);
  localStorage.setItem(themeData.storageKey, theme);
}

export const theme = { get, toggle, apply }
