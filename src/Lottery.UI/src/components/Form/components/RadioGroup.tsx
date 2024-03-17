import { Group, Radio, Stack } from "@mantine/core";
import { useController } from "react-hook-form";
import { RadioGroupProps, WithName } from "../types";

function RadioGroup(props: WithName<RadioGroupProps>) {
  const {
    label,
    name,
    options,
    orientation = "horizontal",
    orientationProps,
    ...rest
  } = props;
  const {
    field,
    fieldState: { error: fieldError },
    formState: { defaultValues },
  } = useController({ name });

  const { onChange, ...restField } = field;

  return (
    <Radio.Group
      id={name}
      label={label}
      error={fieldError?.message?.toString()}
      onChange={(value) => {
        onChange(value ?? defaultValues?.[name]);
      }}
      {...rest}
      {...restField}
    >
      {/* TODO: Reduce duplication */}
      {orientation == "horizontal" ? (
        <Group mt={"xs"}>
          {options.map((option, index) => {
            const { label, value, ...rest } = option;
            return (
              <Radio
                key={`${label}-${index}`}
                value={value}
                label={label}
                {...rest}
              />
            );
          })}
        </Group>
      ) : (
        <Stack mt={"xs"}>
          {options.map((option, index) => {
            const { label, value, ...rest } = option;
            return (
              <Radio
                key={`${label}-${index}`}
                value={value}
                label={label}
                {...rest}
              />
            );
          })}
        </Stack>
      )}
    </Radio.Group>
  );
}

export default RadioGroup;
