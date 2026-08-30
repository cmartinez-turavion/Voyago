const navigation = document.querySelector('#mainNavigation');
if (navigation) {
  navigation.addEventListener('hidden.bs.collapse', () => {
    document.querySelector('[data-bs-target="#mainNavigation"]')?.focus();
  });
}

const fallbackImages = document.querySelectorAll('img[data-fallback-src]');
for (const image of fallbackImages) {
  image.addEventListener('error', () => {
    const fallbackSource = image.dataset.fallbackSrc;
    if (!fallbackSource || image.dataset.fallbackApplied === 'true') {
      return;
    }

    image.dataset.fallbackApplied = 'true';
    image.src = fallbackSource;
  });
}

const submittingForms = document.querySelectorAll('form[method="post"]');
for (const form of submittingForms) {
  form.addEventListener('submit', () => {
    const button = form.querySelector('button[type="submit"]');
    if (!button || button.disabled) {
      return;
    }

    button.disabled = true;
    button.setAttribute('aria-busy', 'true');
    button.dataset.originalText = button.textContent ?? '';
    button.textContent = 'Procesando...';
  });
}

if (!window.matchMedia('(prefers-reduced-motion: reduce)').matches && 'IntersectionObserver' in window) {
  const observer = new IntersectionObserver(entries => {
    for (const entry of entries) {
      if (!entry.isIntersecting) {
        continue;
      }

      entry.target.classList.add('vy-reveal--visible');
      observer.unobserve(entry.target);
    }
  }, { threshold: 0.12 });

  for (const element of document.querySelectorAll('.vy-reveal')) {
    observer.observe(element);
  }
} else {
  for (const element of document.querySelectorAll('.vy-reveal')) {
    element.classList.add('vy-reveal--visible');
  }
}
