import { By, WebDriver } from 'selenium-webdriver';

export class TestPage {
  constructor(private readonly driver: WebDriver) {
  }

  async show(): Promise<void> {
    await this.driver.get('https://andreidbr.github.io/JS30/');
  }

  async clickSomething(): Promise<void> {
    await this.driver.findElement(By.xpath('/html/body/div[2]/div[1]')).click();
  }
}
