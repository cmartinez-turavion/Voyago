const navigation = document.querySelector('#mainNavigation');

if (navigation) {
  navigation.addEventListener('hidden.bs.collapse', () => {
    document.querySelector('[data-bs-target="#mainNavigation"]')?.focus();
  });
}
