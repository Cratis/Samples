import { DataGrid } from './DataGrid';
import { withViewModel } from '@cratis/applications.react.mvvm';
import { HomeViewModel } from './HomeViewModel';
import * as styles from './Home.module.css';

export const Home = withViewModel(HomeViewModel, ({viewModel}) => {
    return (
        <div>
            <h1 className={styles.blah}>Hello world</h1>
            <DataGrid title="The ultimate one" />

            <div>Counter {viewModel.counter}</div>
            <button onClick={() => viewModel.increaseCounter()}>Increase counter</button>
        </div>
    );
});
