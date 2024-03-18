import { useForm } from "@mantine/form";
import {
  CreateGamePrizeRequest,
  createGamePrizeRequestSchema,
} from "./createGame.schema";
import { zodResolver } from "mantine-form-zod-resolver";
import { Group, NumberInput } from "@mantine/core";
import { CloseButton } from "@mantine/core";
import { useEffect } from "react";

interface CreateGamePrizeProps {
  prizeNo: number;
  defaultValues?: Partial<CreateGamePrizeRequest>;
  canRemovePrize: boolean;
  removePrize: (prizeNo: number) => void;
  onChange: (prizeNo: number, prize: CreateGamePrizeRequest) => void;
}

function CreateGamePrize({
  defaultValues,
  canRemovePrize,
  prizeNo,
  onChange,
  removePrize,
}: CreateGamePrizeProps) {
  const form = useForm<CreateGamePrizeRequest>({
    validate: zodResolver(createGamePrizeRequestSchema),
    initialValues: {
      numberMatchCount: 1,
      position: 1,
      ...defaultValues,
    },
  });

  useEffect(() => {
    onChange(prizeNo, form.values);
  }, [form.values]);

  return (
    <Group key={prizeNo}>
      <NumberInput label="Position" {...form.getInputProps("position")} />
      <NumberInput
        label="Matching Numbers"
        {...form.getInputProps("numberMatchCount")}
      />
      {canRemovePrize && <CloseButton onClick={() => removePrize(prizeNo)} />}
    </Group>
  );
}
export default CreateGamePrize;
