import { expect } from 'chai';
import { Builder, WebDriver } from 'selenium-webdriver';
import { TestPage } from './screen-objects';

describe('XXX', () => {
  let driver: WebDriver;

  before(async () => {
      driver = await new Builder().forBrowser('chrome').build();
    });

    it('01 Drums Access Await', async () => {
      var testPage = await new TestPage(driver);
      await testPage.show();
      await testPage.clickSomething();

      const title = await driver.getTitle();

      expect(title).to.equal("JS30: 01 Drums");
    }
  );

  after(async () => {
    await driver.quit()
  });
});
