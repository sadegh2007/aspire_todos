import {createBrowserRouter, createRoutesFromElements, Route, RouterProvider} from "react-router-dom";
import AppLayout from "../layouts/AppLayout.tsx";
import LoginPage from "../features/auth/pages/LoginPage.tsx";
import NotFoundPage from "../common/pages/NotFoundPage.tsx";
import ProtectedRoute from "./ProtectedRoute.tsx";
import HomePage from "../features/todos/pages/HomePage.tsx";

const AppRouter = () => {
  const routes = createBrowserRouter(createRoutesFromElements(
      <Route element={<AppLayout />}>
          <Route path='/login' element={<LoginPage />} />
          <Route index path='/' element={<ProtectedRoute><HomePage /></ProtectedRoute>} />
          <Route path='*' element={<NotFoundPage />} />
      </Route>
  ));
  
  return (<RouterProvider router={routes}/>)
}

export default AppRouter;