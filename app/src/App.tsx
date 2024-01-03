import { QueryClientProvider } from "@tanstack/react-query";
import WidgetList from "./Components/Widgets/WidgetList";
import { queryClient } from "./Utils/QueryClient";

const App = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <WidgetList />
    </QueryClientProvider>
  );
};

export default App;
