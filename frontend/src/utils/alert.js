let alertModalRef = null;

export function setAlertModalRef(ref) {
  alertModalRef = ref;
}

export function showAlert(message, opts = ["Ok"], withInput = false, defaultValue = '') {
  return new Promise((resolve) => {
    if (!alertModalRef) throw new Error("AlertModal not initialized");

    alertModalRef.promptText = message;
    alertModalRef.options = opts;
    alertModalRef.showInput = withInput;
    alertModalRef.inputValue = defaultValue;
    alertModalRef.isVisible = true;
    alertModalRef.resolveFn = resolve;

    if (withInput) {
      setTimeout(() => {
        alertModalRef.inputRef.focus();
      }, 0);
    }
  });
}