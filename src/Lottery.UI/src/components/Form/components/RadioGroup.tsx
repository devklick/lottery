import { Group, Radio, Stack } from "@mantine/core";
import { useController } from "react-hook-form";
import { RadioGroupProps, WithName } from "../types";
import ErrorMessage from "./ErrorMessage";

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

  const error = fieldError ? (
    <ErrorMessage>{fieldError.message?.toString()}</ErrorMessage>
  ) : undefined;

  const { onChange, ...restField } = field;

  return (
    <Radio.Group
      id={name}
      label={label}
      error={error}
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
