import { Modal } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { useMutation } from "@tanstack/react-query";

import SelectionPicker from "./SelectionPicker";
import { notifyInfo, updateNotifySuccess } from "../../../common/notifications";
import gameService from "../gameService";

interface EditEntryProps {
  selectionNumbers: ReadonlyArray<number>;
  selectedNumbers: ReadonlyArray<number>;
  onClose(): void;
  entryId: string;
}

function EditEntry({
  selectionNumbers,
  selectedNumbers,
  entryId,
  onClose,
}: EditEntryProps) {
  const [opened, { close }] = useDisclosure(true);

  const mutation = useMutation({
    mutationFn: gameService.editEntry,
    onSuccess: () => null,
  });

  async function handleSubmit(selectionNumbers: Array<number>) {
    const id = notifyInfo({
      title: "Updating entry",
      message: `Your numbers are being updated (${selectionNumbers.join(", ")})`,
      loading: true,
      autoClose: false,
      allowClose: false,
    });
    await mutation.mutateAsync({
      route: { entryId },
      body: {
        selections: selectionNumbers.map((selectionNumber) => ({
          selectionNumber,
        })),
      },
    });
    updateNotifySuccess(id, {
      title: "Entry updated",
      message: `Your numbers have been updated (${selectionNumbers.join(", ")})`,
      loading: false,
    });

    close();
    onClose();
  }
  function handleClose() {
    close();
    onClose();
  }
  return (
    <Modal opened={opened} onClose={handleClose}>
      <SelectionPicker
        selectionNumbers={selectionNumbers}
        selectedNumbers={selectedNumbers}
        requiredCount={selectedNumbers.length}
        onSubmit={handleSubmit}
        submitStatus={
          mutation.status === "idle"
            ? "waiting"
            : mutation.status === "pending"
              ? "submitting"
              : "success"
        }
      />
    </Modal>
  );
}

export default EditEntry;
