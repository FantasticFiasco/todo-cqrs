export async function condition(predicate: () => Promise<boolean>): Promise<void> {
  while (true) {
    console.log("start while");
    if (await predicate()) {
      console.log("we are done, lets finish");
      return;
    }

    console.log("we are not done, lets wait some more");
    await delay(50);
  }
}

async function delay(milliseconds: number): Promise<void> {
  return new Promise((resolve) => {
    setTimeout(resolve, milliseconds);
  });
}
