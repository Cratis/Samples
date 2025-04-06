import { HomeViewModel } from '../HomeViewModel';

describe('when increasing counter', () => {
    const viewModel = new HomeViewModel(null!);

    viewModel.increaseCounter();

    it('should increase the counter', () => viewModel.counter.should.be.equal(1));
});
