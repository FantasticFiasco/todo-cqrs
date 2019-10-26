export async function condition(predicate: () => Promise<boolean>): Promise<void> {
  while (true) {
    if (await predicate()) {
      return;
    }

    await delay(50);
  }
}

async function delay(milliseconds: number): Promise<void> {
  return new Promise((resolve) => {
    setTimeout(resolve, milliseconds);
  });
}
