import { MultiSelect as _MultiSelect } from "@mantine/core";
import { useController } from "react-hook-form";
import { MultiSelectProps, WithName } from "../types";

function MultiSelect(props: WithName<MultiSelectProps>) {
  const { label, name, ...rest } = props;
  // const [options, setOptions] = useState(props.options);
  const {
    field,
    fieldState: { error: fieldError },
    formState: { defaultValues },
  } = useController({ name });

  const { onChange, ...restField } = field;

  return (
    <_MultiSelect
      id={name}
      label={label}
      data={props.options}
      onChange={(value) => {
        onChange(value ?? defaultValues?.[value]);
      }}
      error={fieldError?.message?.toString()}
      {...rest}
      {...restField}
    />
  );
}

export default MultiSelect;
