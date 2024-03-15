import { Group, Text, TextProps } from "@mantine/core";

const ErrorMessage = (props: TextProps & { children?: string }) => {
  const { children, ...rest } = props;
  if (!children?.length) return null;
  return (
    <Text
      fw={500}
      size="sm"
      style={{
        wordBreak: "break-word",
        display: "block",
        position: "relative",
      }}
      {...rest}
    >
      <Group gap={5} style={{ position: "absolute" }}>
        {children}
      </Group>
    </Text>
  );
};

export default ErrorMessage;
