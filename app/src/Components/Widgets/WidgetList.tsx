import { useQuery } from "@tanstack/react-query";
import Widget from "./Interfaces/Widget";
import WidgetCard from "./WidgetCard";

const WidgetList: React.FC = () => {
  const { isPending, isError, error, data } = useQuery<Widget[]>({
    queryKey: ["widget"],
    queryFn: () =>
      fetch("http://localhost:5194/api/widget").then((res) => res.json()),
  });

  if (isPending) return <div>Loading...</div>;

  if (isError) return <div>Something went wrong...{error.message}</div>;

  return (
    <div>
      {data?.map((widget) => (
        <WidgetCard key={widget.id} widget={widget} />
      ))}
    </div>
  );
};

export default WidgetList;
