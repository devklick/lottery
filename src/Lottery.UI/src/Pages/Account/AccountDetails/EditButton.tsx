import {
  Button,
  ButtonGroup,
  Group,
  Skeleton,
  useMantineTheme,
} from "@mantine/core";
import { useMediaQuery } from "@mantine/hooks";

interface EditButtonProps {
  editing: boolean;
  onEditClicked(): void;
  submitEditDisabled: boolean;
  cancelEditDisabled: boolean;
  onCancelEditClicked(): void;
  loading?: boolean;
}

export default function EditButton({
  cancelEditDisabled,
  editing,
  onCancelEditClicked,
  onEditClicked,
  submitEditDisabled,
  loading,
}: EditButtonProps) {
  const { breakpoints } = useMantineTheme();
  const sm = useMediaQuery(`(max-width: ${breakpoints.sm})`);
  const xs = useMediaQuery(`(max-width: ${breakpoints.xs})`);
  return (
    <Group justify="flex-end" w="100%">
      {!editing && (
        <Skeleton visible={loading} w={"fit-content"}>
          <Button onClick={onEditClicked}>Edit</Button>
        </Skeleton>
      )}
      {editing && (
        <ButtonGroup
          orientation={xs ? "vertical" : "horizontal"}
          w={sm ? "100%" : "auto"}
        >
          <Button
            disabled={cancelEditDisabled}
            type="reset"
            onClick={onCancelEditClicked}
            fullWidth={sm}
            variant="outline"
            color="red"
          >
            Cancel
          </Button>
          <Button
            disabled={submitEditDisabled}
            type="submit"
            variant="gradient"
            gradient={{ from: "blue", to: "grape", deg: 20 }}
            fullWidth={sm}
            form={"update-account"}
          >
            Submit
          </Button>
        </ButtonGroup>
      )}
    </Group>
  );
}
