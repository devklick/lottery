import { Select as MantineSelect } from "@mantine/core";
import { useController } from "react-hook-form";
import { SelectProps, WithName } from "../types";

function Select(props: WithName<SelectProps>) {
  const { label, options, name, ...rest } = props;

  const {
    field,
    fieldState: { error: fieldError },
    formState: { defaultValues },
  } = useController({ name });

  const { onChange, ...restField } = field;

  return (
    <MantineSelect
      id={name}
      // rightSection={<IconChevronDown width={15} color="#9e9e9e" />}
      // styles={{ rightSection: { pointerEvents: "none" } }}
      label={label}
      onChange={(value) => onChange(value ?? defaultValues?.[name])}
      allowDeselect
      error={fieldError?.message?.toString()}
      {...rest}
      data={options}
      {...restField}
    />
  );
}

export default Select;