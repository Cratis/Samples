import { useTheme } from './Utils/useTheme';
import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { LayoutProvider } from './Layout/Default/context/LayoutContext';
import { DialogComponents } from '@cratis/applications.react.mvvm/dialogs';
import { ConfirmationDialog } from 'Components/Dialogs';
import { Home } from './Home';
import { DefaultLayout } from './Layout/Default/DefaultLayout';

function App() {
    useTheme();
    return (
        <LayoutProvider>
            <DialogComponents confirmation={ConfirmationDialog}>
                <BrowserRouter>
                    <Routes>
                        <Route path='/' element={<Navigate to={'/home'} />} />
                        <Route path='/home' element={<DefaultLayout />}>
                            <Route path={''} element={<Home />} />
                        </Route>
                    </Routes>
                </BrowserRouter>
            </DialogComponents>
        </LayoutProvider>
    );
}

export default App;
