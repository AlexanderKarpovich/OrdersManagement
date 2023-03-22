import { BrowserRouter, NavLink, Navigate, Route, Routes } from "react-router-dom";
import OrdersPage from "./components/pages/home/OrdersPage";
import ProvidersPage from "./components/pages/providers/ProvidersPage";
import CreateOrderPage from './components/pages/create/CreateOrderPage';
import ReadOrderPage from "./components/pages/read/ReadOrderPage";

function App() {
  return (
    <BrowserRouter>
      <div className="w-full bg-black text-white text-end p-2">
        <div className="text-lg p-2 flex justify-end">
          <NavLink
            to='/'
            className={
              ({ isActive, isPending }) =>
                isActive ? "bg-gray-600 rounded px-4 py-2 hover:bg-gray-700" :
                  isPending ? "text-gray-500" :
                    "px-4 py-2 hover:text-gray-300"}
          >
            Главная
          </NavLink>
          <NavLink
            to='/orders'
            className={
              ({ isActive, isPending }) =>
                isActive ? "bg-gray-600 rounded px-4 py-2 hover:bg-gray-700" :
                  isPending ? "text-gray-500" :
                    "px-4 py-2 hover:text-gray-300"}
            end
          >
            Заказы
          </NavLink>
          <NavLink
            to='/providers'
            className={
              ({ isActive, isPending }) =>
                isActive ? "bg-gray-600 rounded px-4 py-2 hover:bg-gray-700" :
                  isPending ? "text-gray-500" :
                    "px-4 py-2 hover:text-gray-300"}
          >
            Поставщики
          </NavLink>
        </div>
      </div>
      <Routes>
        <Route path={'/'} element={<OrdersPage />} />
        <Route path={'/orders'} element={<OrdersPage />} />
        <Route path={'/providers'} element={<ProvidersPage />} />
        <Route path={'/orders/:id'} element={<ReadOrderPage />} />
        <Route path={'/orders/:id/edit'} element={<CreateOrderPage />} />
        <Route path={'/orders/create'} element={<CreateOrderPage />} />
        <Route path="*" element={<Navigate to="/" />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;