import Widget from "./Interfaces/Widget";

const WidgetCard: React.FC<{ widget: Widget }> = ({ widget }) => {
  return (
    <div>
      <h1>{widget.widgetName}</h1>
    </div>
  );
};

export default WidgetCard;
