// Utility functions for loading state management will go here.

export function addLoadingOverlay(element) {
    if (element.classList.contains('loading')) return;
    element.classList.add('loading');
}

export function removeLoadingOverlay(element) {
    element.classList.remove('loading');
}
